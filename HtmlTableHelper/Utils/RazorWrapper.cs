using System.Text;
using System.Web.Mvc;
using HtmlTableHelper.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTableHelper.Utils
{
    public static class RazorWrapper
    {
        private static readonly StringBuilder Str = new StringBuilder();

        public static MvcHtmlString RenderTable(TableViewModel table)
        {
            var razorResult = Engine.Razor.RunCompile(FilesManager.TableView, "table", null, table);
            Str.Clear();
            Str.Append(razorResult);
            return MvcHtmlString.Create(Str.ToString());
        }
    }
}