using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HtmlTableHelper.Filters;
using HtmlTableHelper.Models;
using HtmlTableHelper.Utils;
using HtmlTableHelper.ViewModel;

namespace HtmlTableHelper.Logic
{
    public class HtmlTable<TRowModel>
    {
        #region UTILS

        #region PROPERTIES
        private readonly TableViewModel _table = new TableViewModel();
        private readonly IEnumerable<TRowModel> _rows;
        protected readonly TextWriter Writer;
        private readonly object _model;
        public DisposableHtmlTable<TRowModel> Begin => new DisposableHtmlTable<TRowModel>(_model, _rows, Writer);

        #endregion

        public HtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer)
        {
            _rows = rows as IList<TRowModel> ?? rows.ToList();
            Writer = writer;
            _model = model;

            _table.Rows = new List<List<string>>();
            _table.Header = typeof(TRowModel).GetProperties().Select(p => p.Name).ToList();
        }

        private void GenerateRows()
        {
            //For each row, add the value of each column to the model
            foreach (var row in _rows)
            {
                var values = _table.Header.Select(col => typeof(TRowModel).GetProperty(col)?.GetValue(row, null).ToString()).ToList();
                _table.Rows.Add(values);
            }
        }

        public IHtmlString Render()
        {
            GenerateRows();
            return RazorWrapper.RenderTable(_table);
        }

        public HtmlTable<TRowModel> Exclude<TCol>(Expression<Func<TRowModel, TCol>> expression)
        {
            var member = expression.Body as MemberExpression;
            var propertyName = member?.Member.Name;
            _table.Header.Remove(propertyName);

            return this;
        }

        #endregion

        #region RENAME

        public HtmlTable<TRowModel> Rename<TCol>(Expression<Func<TRowModel, TCol>> expression, string newName)
        {
            var baseName = (expression.Body as MemberExpression)?.Member.Name;
            if (baseName == null)
                throw new ArgumentException("The provided column could not be found");

            _table.GlobalRenameMapping.Add(baseName, newName);

            return this;
        }

        public HtmlTable<TRowModel> Rename<TCol>(Expression<Func<TRowModel, TCol>> expression, string newName, Table.Part part)
        {
            if (part == Table.Part.Header)
                RenameHeader(expression, newName);

            if (part == Table.Part.Footer)
                RenameFooter(expression, newName);

            return this;
        }

        public HtmlTable<TRowModel> RenameHeader<TCol>(Expression<Func<TRowModel, TCol>> expression, string newName)
        {
            var baseName = (expression.Body as MemberExpression)?.Member.Name;
            if (baseName == null)
                throw new ArgumentException("The provided column could not be found");

            _table.HeaderRenameMapping.Add(baseName, newName);

            return this;
        }

        public HtmlTable<TRowModel> RenameFooter<TCol>(Expression<Func<TRowModel, TCol>> expression, string newName)
        {
            var baseName = (expression.Body as MemberExpression)?.Member.Name;
            if (baseName == null)
                throw new ArgumentException("The provided column could not be found");

            _table.FooterRenameMapping.Add(baseName, newName);

            return this;
        }

        #endregion

        #region FITLERS

        public HtmlTable<TRowModel> ColFilter<TCol>(IColFilter colFilter, Expression<Func<TRowModel, TCol>> expression)
        {
            var propertyName = (expression.Body as MemberExpression)?.Member.Name;
            if (propertyName == null)
                throw new ArgumentException("The provided column could not be found");

            _table.FiltersMapping.Remove(propertyName);

            if (colFilter != null)
                _table.FiltersMapping.Add(propertyName, colFilter);

            return this;
        }

        public HtmlTable<TRowModel> ColsFilter<TCol>(IColFilter colFilter, params Expression<Func<TRowModel, TCol>>[] expressions)
        {
            foreach (var expression in expressions)
                ColFilter(colFilter, expression);


            return this;
        }

        public HtmlTable<TRowModel> RemoveColsFilter<TCol>(params Expression<Func<TRowModel, TCol>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyName = (expression.Body as MemberExpression)?.Member.Name;
                if (propertyName == null)
                    throw new ArgumentException("The provided column could not be found");

                _table.FiltersMapping.Remove(propertyName);
            }

            return this;
        }

        public HtmlTable<TRowModel> ClearColsFilter()
        {
            _table.FiltersMapping.Clear();

            return this;
        }

        #endregion

        #region PARTS

        public HtmlTable<TRowModel> Disable(Table.Part part)
        {
            Toggle(part, false);

            return this;
        }

        public HtmlTable<TRowModel> Enable(Table.Part part)
        {
            Toggle(part, true);

            return this;
        }

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

        public HtmlTable<TRowModel> Class(string classes)
        {
            _table.RootClasses = classes;

            return this;
        }

        #endregion
    }
}
