using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using HtmlTableHelper.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTableHelper
{
    public class HtmlTable<TValue>
    {
        private TValue _model;
        private StringBuilder _str = new StringBuilder();
        private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        public HtmlTable(TValue model)
        {
            Init(model);
        }

        public HtmlTable<TValue> Init(TValue model)
        {
            _model = model;

            return this;
        }

        public string Render()
        {
            _str.Append(Engine.Razor.RunCompile(File.ReadAllText($"{ViewsPath}/Table.cshtml"), "table", null, new TableViewModel()));
            return _str.ToString();
        }
    }
}