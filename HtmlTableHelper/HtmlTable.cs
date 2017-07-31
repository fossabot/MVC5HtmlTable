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
    public class HtmlTable<TValue> where TValue : IList
    {
        private TValue _inputModel;
        private readonly TableViewModel _outputViewModel = new TableViewModel();
        private StringBuilder _str = new StringBuilder();
        private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        public HtmlTable(TValue model)
        {
            Init(model);
        }

        public HtmlTable<TValue> Init(TValue model)
        {
            if(model.Count == 0)
                throw new ArgumentException("The list must not be empty");

            _inputModel = model;

            UpdateViewModel();

            return this;
        }

        private void UpdateViewModel()
        {
            var subType = _inputModel.GetContainedType();

            var properties = subType.GetProperties();
            _outputViewModel.Header = properties.Select(p => p.Name).ToList();
        }

        public IHtmlString Render()
        {
            var razorRaw = File.ReadAllText($"{ViewsPath}/Table.cshtml");
            var razorResult = Engine.Razor.RunCompile(razorRaw, "table", null, _outputViewModel);
            _str.Append(razorResult);
            return MvcHtmlString.Create(_str.ToString());
        }
    }
}