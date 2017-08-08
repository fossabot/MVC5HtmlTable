using System.Text;
using System.Web.Mvc;
using HtmlTable.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTable.Utils
{
    internal static class RazorWrapper
    {
        private static readonly StringBuilder Str = new StringBuilder();

        public static MvcHtmlString RenderTable(TableViewModel table)
        {
            var razorResult = Engine.Razor.RunCompile(FilesManager.PathToTableRazorView, "table", null, table);
            Str.Clear();
            Str.Append(razorResult);
            return MvcHtmlString.Create(Str.ToString());
        }
    }
}