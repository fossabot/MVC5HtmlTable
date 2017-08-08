using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HtmlTable.Models;
using HtmlTable.Strategies.Converters;
using HtmlTable.Strategies.Filters;
using HtmlTable.Strategies.Filters.Predefined;
using HtmlTable.Strategies.Injectors;
using HtmlTable.Strategies.Injectors.Predefined;
using HtmlTable.Utils;
using HtmlTable.ViewModel;

namespace HtmlTable.Logic
{
    /// <summary>
    /// Container for the fluent API - Generate the HTML table by calling <see cref="Render()"/>
    /// <example>
    /// Basic example generating a HTML table in a Razor MVC5 view where the model containes a property ListTest that is an IEnumerable
    /// <code>
    /// @Html.DisplayTable(m => m.ListTest).Render();
    /// </code>
    /// </example>
    /// </summary>
    /// <seealso cref="TableHelpers"/>
    /// <typeparam name="TRowModel">type used to contain a row in the user's source data</typeparam>
    public class HtmlTable<TRowModel>
    {
        #region UTILS

        #region PROPERTIES

        /// <summary>
        /// Contains the final settings for the razor engine to render the HTML talbe
        /// </summary>
        private readonly TableViewModel _table = new TableViewModel();
        /// <summary>
        /// Contains the final data for the razor engine to fill the HTML table
        /// </summary>
        private readonly IEnumerable<TRowModel> _rows;
        /// <summary>
        /// Contains the definition for the virtual columns. <see cref="AddColumn(string,HtmlTable.Strategies.Injectors.IColDataInjector)"/>
        /// </summary>
        private readonly Dictionary<string, IColDataInjector> _addedColumnsMappin = new Dictionary<string, IColDataInjector>();
        /// <summary>
        /// The razor ViewModel - used to allow strong typing of generics and lambdas
        /// </summary>
        private readonly object _model;

        protected readonly TextWriter Writer;

        /// <summary>
        /// Returns a <see cref="DisposableHtmlTable{TRowModel}"/> builts from the current <see cref="HtmlTable{TRowModel}"/>, which implements the <see cref="IDisposable"/> interface to be used with the <c>using()</c> syntaxe.
        /// When created this way, there is no need to call <see cref="Render"/>, this will be automatically handled the object is being disposed
        /// <example>
        /// <code>
        /// @using (var table = Html.DisplayTable(m => m.ListTest).Begin)
        /// {
        ///     // Add code to configure the HTML tabel
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public DisposableHtmlTable<TRowModel> Begin => new DisposableHtmlTable<TRowModel>(_model, _rows, Writer);

        #endregion

        /// <summary>
        /// Initializes an object
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rows">Creates a new list of <typeparam name="TRowModel">row type</typeparam></param>
        /// <param name="writer">Only used when the <see cref="DisposableHtmlTable{TRowModel}"/> object is used through a call to begin <see cref="Begin"/> to render the table without a need to call <see cref="Render()"/></param>
        public HtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer)
        {
            _rows = rows as IList<TRowModel> ?? rows.ToList();
            Writer = writer;
            _model = model;

            _table.Rows = new List<List<IColDataInjector>>();
            _table.ColumnsName = typeof(TRowModel).GetProperties().Select(p => p.Name).ToList();
        }

        /// <summary>
        /// Update the view model, inserts the data rows using the give configuration
        /// </summary>
        private void GenerateRows()
        {
            //For each row, add the value of each column to the model
            foreach (var row in _rows)
            {
                var values = _table.ColumnsName.Select(col => GetColValueInRow(col, row)).ToList();
                _table.Rows.Add(values);

            }
        }

        /// <summary>
        /// Get the value for a given column in a given row (from the use)
        /// </summary>
        /// <param name="col">The string name of the column (name of the property)</param>
        /// <param name="row">The row from which to extract the data</param>
        /// <returns></returns>
        private IColDataInjector GetColValueInRow(string col, TRowModel row)
        {
            var property = typeof(TRowModel).GetProperty(col);


            var value = property?.GetValue(row, null).ToString();

            if (value == null && _addedColumnsMappin.ContainsKey(col))
                return _addedColumnsMappin[col];


            return new StringColDataInjector(value);
        }

        /// <summary>
        /// Renders the <see cref="HtmlTable{TRowModel}"/> to a string containing the HTML table generated from the provided config
        /// It must be used as a final call to generate the table, except when using <see cref="Begin"/> and <see cref="DisposableHtmlTable{TRowModel}"/>.
        /// See <see cref="Begin"/> for more information about this specific aproach
        /// </summary>
        /// <returns>String HTML table - ready to render in browser</returns>
        public IHtmlString Render()
        {
            GenerateRows();
            return RazorWrapper.RenderTable(_table);
        }

        /// <summary>
        /// Removes a column from the final render
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="expression"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Exclude<TCol>(Expression<Func<TRowModel, TCol>> expression)
        {
            var propertyName = GetPropertyName(expression);
            _table.ColumnsName.Remove(propertyName);

            return this;
        }

        /// <summary>
        /// Returns a string containing the name of the property tardeted by the lambda expression
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="expression">A lambda expression targetting a property like <c>table.Exclude(rowModel =&gt; rowModel.Col1)</c></param>
        /// <returns></returns>
        private static string GetPropertyName<TCol>(Expression<Func<TRowModel, TCol>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("The provided column does not be exist");
            var propertyName = member.Member.Name;
            return propertyName;
        }

        #endregion

        #region RENAME

        /// <summary>
        /// Changes the name of a property displayed in both the header and footer of the HTML table. This property will be overridden by the <see cref="RenameHeader{TCol}"/> and <see cref="RenameFooter{TCol}"/> methods.
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpression"></param>
        /// <param name="newName"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Rename<TCol>(Expression<Func<TRowModel, TCol>> targetPropertyExpression, string newName)
        {
            _table.GlobalRenameMapping.Add(GetPropertyName(targetPropertyExpression), newName);

            return this;
        }

        /// <summary>
        /// Changes the name of a property displayed in the header of the HTML table. This will override the setting defined in <see cref="Rename{TCol}(Expression{Func{TRowModel,TCol}},string)"/>
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpression"></param>
        /// <param name="newName"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> RenameHeader<TCol>(Expression<Func<TRowModel, TCol>> targetPropertyExpression, string newName)
        {
            _table.HeaderRenameMapping.Add(GetPropertyName(targetPropertyExpression), newName);

            return this;
        }

        /// <summary>
        /// Changes the name of a property displayed in the footer of the HTML table. This will override the setting defined in <see>
        ///         <cref>Rename{TCol}(Expression{Func{TRowModel,TCol}},string)</cref>
        ///     </see>
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpression"></param>
        /// <param name="newName"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> RenameFooter<TCol>(Expression<Func<TRowModel, TCol>> targetPropertyExpression, string newName)
        {
            _table.FooterRenameMapping.Add(GetPropertyName(targetPropertyExpression), newName);

            return this;
        }

        /// <summary>
        /// Alias for <see cref="RenameHeader{TCol}"/> and <see cref="RenameFooter{TCol}"/> methods. The <paramref name="part"/> param decides chich method to call
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpression"></param>
        /// <param name="newName"></param>
        /// <param name="part"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Rename<TCol>(Expression<Func<TRowModel, TCol>> targetPropertyExpression, string newName, Table.Part part)
        {
            if (part == Table.Part.Header)
                RenameHeader(targetPropertyExpression, newName);

            if (part == Table.Part.Footer)
                RenameFooter(targetPropertyExpression, newName);

            return this;
        }

        #endregion

        #region FITLERS

        /// <summary>
        /// Allows to add a custom filtering strategy on a column. Implement the <see cref="IColFilter"/> interface to pass a custom made filter to <paramref name="colFilter"/> or use the pre-defined filters located in <see cref="Strategies.Filters.Predefined"/> namespace
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colFilter">An implementation of <see cref="IColFilter"/>. Make your own or use the ones predefined in the <see cref="Strategies.Filters.Predefined"/> namespace</param>
        /// <param name="targetPropertyExpression"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColFilter<TCol>(IColFilter colFilter, Expression<Func<TRowModel, TCol>> targetPropertyExpression)
        {
            var propertyName = GetPropertyName(targetPropertyExpression);

            _table.FiltersMapping.Remove(propertyName);

            if (colFilter != null)
                _table.FiltersMapping.Add(propertyName, colFilter);

            return this;
        }

        /// <summary>
        /// Allows to add a custom filtering strategy on multiple columns. Implement the <see cref="IColFilter"/> interface to pass a custom made filter to <paramref name="colFilter"/> or use the pre-defined filters located in <see cref="Strategies.Filters.Predefined"/> namespace.
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colFilter">An implementation of <see cref="IColFilter"/>. Make your own or use the ones predefined in the <see cref="Strategies.Filters.Predefined"/> namespace</param>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColsFilter<TCol>(IColFilter colFilter, params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
                ColFilter(colFilter, targetPropertyExpression);


            return this;
        }

        /// <summary>
        /// Removes the filter defined on a column
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> RemoveColsFilter<TCol>(params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
            {

                _table.FiltersMapping.Remove(GetPropertyName(targetPropertyExpression));
            }

            return this;
        }

        /// <summary>
        /// Removes the filters on every column
        /// </summary>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ClearColsFilter()
        {
            _table.FiltersMapping.Clear();

            return this;
        }

        #endregion

        #region INJECTORS

        /// <summary>
        /// Add a virtual column to the table.
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="injector">An implementation of <see cref="IColDataInjector"/>. Make your own or use the ones predefined in the <see cref="Strategies.Filters.Predefined"/> namespace</param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> AddColumn(string colName, IColDataInjector injector)
        {
            _table.ColumnsName.Remove(colName);
            _table.ColumnsName.Add(colName);
            _addedColumnsMappin.Add(colName, injector);

            return this;
        }

        /// <summary>
        /// Alias for <see cref="AddColumn(string,HtmlTable.Strategies.Injectors.IColDataInjector)"/> 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="injector">An implementation of <see cref="IColDataInjector"/>. Make your own or use the ones predefined in the <see cref="Strategies.Filters.Predefined"/> namespace</param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> SetColumn(string colName, IColDataInjector injector)
        {
            AddColumn(colName, injector);
            return this;
        }

        /// <summary>
        /// Add a virtual column to the table.
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="injectorValue">Will be converted to an <see cref="StringColDataInjector"/></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> AddColumn(string colName, string injectorValue)
        {
            AddColumn(colName, new StringColDataInjector(injectorValue));

            return this;
        }

        /// <summary>
        /// Alias for <see cref="AddColumn(string,string)"/>
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="injectorValue">Will be converted to an <see cref="StringColDataInjector"/></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> SetColumn(string colName, string injectorValue)
        {
            AddColumn(colName, new StringColDataInjector(injectorValue));
            return this;
        }

        #endregion

        #region CONVERTERS

        /// <summary>
        /// Alters the output data for the given column. Implement the <see cref="IColConverter"/> interface to pass a custom made filter to <paramref name="colConverter"/> or use the pre-defined filters located in <see cref="Strategies.Converters.Predefined"/> namespace.
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colConverter">An implementation of <see cref="IColConverter"/>. Transforms the column's data</param>
        /// <param name="targetPropertyExpression"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColConverter<TCol>(IColConverter colConverter, Expression<Func<TRowModel, TCol>> targetPropertyExpression)
        {
            var propertyName = GetPropertyName(targetPropertyExpression);

            _table.ConvertersMapping.Remove(propertyName);

            if (colConverter != null)
                _table.ConvertersMapping.Add(propertyName, colConverter);

            return this;
        }

        /// <summary>
        /// Alters the output data for the given columns. Implement the <see cref="IColConverter"/> interface to pass a custom made filter to <paramref name="colConverter"/> or use the pre-defined filters located in <see cref="Strategies.Converters.Predefined"/> namespace.
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colConverter">An implementation of <see cref="IColConverter"/>. Transforms the column's data</param>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColsConverter<TCol>(IColConverter colConverter, params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
                ColConverter(colConverter, targetPropertyExpression);


            return this;
        }

        /// <summary>
        /// Removes the <see cref="IColConverter"/> from the given column
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> RemoveColsConverter<TCol>(params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
            {

                _table.ConvertersMapping.Remove(GetPropertyName(targetPropertyExpression));
            }

            return this;
        }

        /// <summary>
        /// Remove every <see cref="IColConverter"/> on every column
        /// </summary>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ClearColsConverter()
        {
            _table.ConvertersMapping.Clear();

            return this;
        }

        #endregion

        #region PARTS

        /// <summary>
        /// Hides a given part of the table (thead/tbody)
        /// </summary>
        /// <param name="part"><see cref="Table.Part.Header"/> or <see cref="Table.Part.Footer"/></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Disable(Table.Part part)
        {
            Toggle(part, false);

            return this;
        }

        /// <summary>
        /// Shows a given part of the table (thead/tbody)
        /// </summary>
        /// <param name="part"><see cref="Table.Part.Header"/> or <see cref="Table.Part.Footer"/></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Enable(Table.Part part)
        {
            Toggle(part, true);

            return this;
        }

        /// <summary>
        /// Hides/Shows a given part of the table. If <paramref name="val"/> is null the part specified via the <paramref name="part"/> parameter will be hidden if it was previously displayed, and displayed if it was previously hiddne. If the <paramref name="val"/> parameter has a value: when it is <c>false</c>, the <see cref="Disable"/> method will be called. When it is <c>true</c> the <see cref="Enable"/> method will be called
        /// </summary>
        /// <param name="part"></param>
        /// <param name="val"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Toggle(Table.Part part, bool? val = null)
        {
            bool value;

            if (val == null)
            {
                if (_table.TableOptions.PartsStatus.ContainsKey(part) && _table.TableOptions.PartsStatus[part])
                    value = false;
                else
                    value = true;
            }

            else
                value = (bool)val;

            if (_table.TableOptions.PartsStatus.ContainsKey(part))
                _table.TableOptions.PartsStatus[part] = value;

            else
                _table.TableOptions.PartsStatus.Add(part, value);

            return this;
        }

        #endregion

        #region DESIGN

        /// <summary>
        /// Adds a space separated list of CSS classes on the table element
        /// </summary>
        /// <param name="classes"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> Class(string classes)
        {
            _table.RootClasses = classes;

            return this;
        }

        #region TD

        /// <summary>
        /// Add CSS classes to a given td element 
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colFilter">An implementation of <see cref="IColFilter"/>. Decides when the css class should be applied</param>
        /// <param name="classes">A space separated list of css class</param>
        /// <param name="targetPropertyExpression"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColClass<TCol>(IColFilter colFilter, string classes, Expression<Func<TRowModel, TCol>> targetPropertyExpression)
        {
            var propertyName = GetPropertyName(targetPropertyExpression);

            _table.ColsClassMapping.Remove(propertyName);

            if (colFilter != null)
                _table.ColsClassMapping.Add(propertyName, new ColClassFilter(colFilter, classes));

            return this;
        }

        /// <summary>
        /// Add CSS classes to a list of properties
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="colFilter">An implementation of <see cref="IColFilter"/>. Decides when the css class should be applied</param>
        /// <param name="classes">A space separated list of css class</param>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ColsClass<TCol>(IColFilter colFilter, string classes, params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
                ColClass(colFilter, classes, targetPropertyExpression);

            return this;
        }

        /// <summary>
        /// Removes the CSS classes for the given columns
        /// </summary>
        /// <typeparam name="TCol"></typeparam>
        /// <param name="targetPropertyExpressions"></param>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> RemoveColsClass<TCol>(params Expression<Func<TRowModel, TCol>>[] targetPropertyExpressions)
        {
            foreach (var targetPropertyExpression in targetPropertyExpressions)
            {

                _table.ColsClassMapping.Remove(GetPropertyName(targetPropertyExpression));
            }

            return this;
        }

        /// <summary>
        /// Remove the CSS classes for every column
        /// </summary>
        /// <returns>Returns the current <see cref="HtmlTable{TRowModel}"/> instance to allow method chaining</returns>
        public HtmlTable<TRowModel> ClearColsClass()
        {
            _table.ColsClassMapping.Clear();

            return this;
        }

        #endregion

        #endregion
    }
}
