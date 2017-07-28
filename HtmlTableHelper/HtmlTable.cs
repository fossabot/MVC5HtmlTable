using System;
using System.IO;
using System.Reflection;
using System.Text;
using HtmlTableHelper.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTableHelper
{
    public class HtmlTable
    {
        private StringBuilder _str = new StringBuilder();
        private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        public HtmlTable()
        {
            _str.Append(Engine.Razor.RunCompile(File.ReadAllText($"{ViewsPath}/Table.cshtml"), "table", null, new TableViewModel()));
        }

        public string Render()
        {
            return _str.ToString();
        }
    }
}