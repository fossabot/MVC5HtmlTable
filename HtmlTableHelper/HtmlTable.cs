﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HtmlTableHelper.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTableHelper
{
    public class HtmlTable<TRowModel>
    {
        private readonly TableViewModel _table = new TableViewModel();
        private readonly StringBuilder _str = new StringBuilder();
        private readonly IEnumerable<TRowModel> _rows;
        private readonly string _viewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        public HtmlTable(object model, IEnumerable<TRowModel> rows)
        {
            _rows = rows as IList<TRowModel> ?? rows.ToList();

            _table.Rows = new List<List<string>>();
            _table.Header = typeof(TRowModel).GetProperties().Select(p => p.Name).ToList();
            _table.HeaderRenameMapping = new Dictionary<string, string>();
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

            _table.HeaderRenameMapping.Add(baseName, newName);

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
                value = (bool) val;

            if (_table.TableOptions.PartsStatus.ContainsKey(part))
                _table.TableOptions.PartsStatus[part] = value;

            else
                _table.TableOptions.PartsStatus.Add(part, value);

            return this;
        }

        public IHtmlString Render()
        {
            GenerateRows();

            var razorRaw = File.ReadAllText($"{_viewsPath}/Table.cshtml");
            var razorResult = Engine.Razor.RunCompile(razorRaw, "table", null, _table);
            _str.Append(razorResult);
            return MvcHtmlString.Create(_str.ToString());
        }
    }
}
