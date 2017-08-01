using System;
using System.Collections;
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
        private readonly TableViewModel table = new TableViewModel();
        private readonly StringBuilder _str = new StringBuilder();
        private object _model = null;
        private IEnumerable<TRowModel> _rows = null;
        private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        private PropertyInfo[] properties => typeof(TRowModel).GetProperties();


        public HtmlTable(object model, IEnumerable<TRowModel> rows)
        {
            _model = model;
            _rows = rows as IList<TRowModel> ?? rows.ToList();

            table.Rows = new List<List<string>>();
            table.Header = typeof(TRowModel).GetProperties().Select(p => p.Name).ToList();
            table.HeaderRenameMapping = new Dictionary<string, string>();
        }

        private void GenerateRows()
        {
            //For each row, add the value of each column to the model
            foreach (var row in _rows)
            {
                var values = table.Header.Select(col => typeof(TRowModel).GetProperty(col)?.GetValue(row, null).ToString()).ToList();
                table.Rows.Add(values);
            }
        }

        public HtmlTable<TRowModel> Exclude<TCol>(Expression<Func<TRowModel, TCol>> expression)
        {
            var member = expression.Body as MemberExpression;
            var propertyName = member?.Member.Name;
            table.Header.Remove(propertyName);

            return this;
        }

        public HtmlTable<TRowModel> Rename<TCol>(Expression<Func<TRowModel, TCol>> expression, string newName)
        {
            var baseName = (expression.Body as MemberExpression)?.Member.Name;
            if(baseName == null)
                throw new ArgumentException("The provided column could not be found");

            table.HeaderRenameMapping.Add(baseName, newName);
            
            return this;
        }

        public IHtmlString Render()
        {
            GenerateRows();

            var razorRaw = File.ReadAllText($"{ViewsPath}/Table.cshtml");
            var razorResult = Engine.Razor.RunCompile(razorRaw, "table", null, table);
            _str.Append(razorResult);
            return MvcHtmlString.Create(_str.ToString());
        }
    }
}

//public class HtmlTable<TListModel> where TListModel : IList
//{
//    private TListModel _inputModel;
//    private readonly TableViewModel _outputViewModel = new TableViewModel();
//    private StringBuilder _str = new StringBuilder();
//    private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

//    public HtmlTable(TListModel model)
//    {
//        Init(model);
//    }

//    public HtmlTable<TListModel> Init(TListModel model)
//    {
//        if (model.Count == 0)
//            throw new ArgumentException("The list must not be empty");

//        _inputModel = model;

//        UpdateViewModel();

//        return this;
//    }

//    private void UpdateViewModel()
//    {
//        var subType = _inputModel.GetContainedType();

//        var properties = subType.GetProperties();
//        _outputViewModel.Header = properties.Select(p => p.Name).ToList();
//        _outputViewModel.Rows = new List<List<string>>();
//        foreach (var row in _inputModel)
//        {
//            var values = _outputViewModel.Header.Select(col => subType.GetProperty(col).GetValue(row, null).ToString()).ToList();
//            _outputViewModel.Rows.Add(values);
//        }
//    }

//    public IHtmlString Render()
//    {
//        var razorRaw = File.ReadAllText($"{ViewsPath}/Table.cshtml");
//        var razorResult = Engine.Razor.RunCompile(razorRaw, "table", null, _outputViewModel);
//        _str.Append(razorResult);
//        return MvcHtmlString.Create(_str.ToString());
//    }
//}