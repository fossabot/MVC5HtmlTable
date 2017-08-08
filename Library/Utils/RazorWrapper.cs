using System.Text;
using System.Web.Mvc;
using HtmlTable.ViewModel;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTable.Utils
{
    /// <summary>
    /// Simple build process to compile a Razor view that consumes a <see cref="TableViewModel"/> view model and outputs a <see cref="MvcHtmlString"/> to be rendered in an other view
    /// </summary>
    internal static class RazorWrapper
    {
        /// <summary>
        /// Contains the temporary result of the Razor compilation before creating the result <see cref="MvcHtmlString"/>
        /// </summary>
        private static readonly StringBuilder Str = new StringBuilder();

        /// <summary>
        /// Compiles the Razor view using a <see cref="TableViewModel"/>
        /// </summary>
        /// <param name="table"></param>
        /// <returns><see cref="MvcHtmlString"/> to be rendered in an other Razor view</returns>
        public static MvcHtmlString RenderTable(TableViewModel table)
        {
            // Remove the result of the previous compilation
            Str.Clear();

            var razorResult = Engine.Razor.RunCompile(FilesManager.PathToTableRazorView, "table", null, table);

            Str.Append(razorResult);

            // Return an MvcHtmlString to avoid having to call .Raw() in the calling view
            return MvcHtmlString.Create(Str.ToString());
        }
    }
}