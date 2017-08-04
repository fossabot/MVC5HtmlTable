using System.Text;
using System.Web.Mvc;
using Library.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace Library.Utils
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