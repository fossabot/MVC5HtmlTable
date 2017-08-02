using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HtmlTableHelper.ViewModel;

namespace HtmlTableHelper
{
    public class HtmlTable<TRowModel>
    {
        private readonly TableViewModel _table = new TableViewModel();
        private readonly IEnumerable<TRowModel> _rows;
        protected readonly TextWriter _writer;
        private readonly object _model;
        public DisposableHtmlTable<TRowModel> Begin => new DisposableHtmlTable<TRowModel>(_model, _rows, _writer);

        public HtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer)
        {
            _rows = rows as IList<TRowModel> ?? rows.ToList();
            _writer = writer;
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

        public HtmlTable<TRowModel> Exclude<TCol>(Expression<Func<TRowModel, TCol>> expression)
        {
            var member = expression.Body as MemberExpression;
            var propertyName = member?.Member.Name;
            _table.Header.Remove(propertyName);

            return this;
        }

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

        public HtmlTable<TRowModel> Class(string classes)
        {
            _table.RootClasses = classes;

            return this;
        }

        public IHtmlString Render()
        {
            GenerateRows();
            return RazorWrapper.RenderTable(_table);
        }
    }

    public class DisposableHtmlTable<TRowModel> : HtmlTable<TRowModel>, IDisposable
    {
        public DisposableHtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer) : base(model, rows, writer)
        {

        }

        public void Dispose()
        {
            _writer.Write(Render());
        }
    }
}
