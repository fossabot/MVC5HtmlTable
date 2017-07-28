using System.Web.Mvc;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;

namespace HtmlTableHelper
{
    public static class HelperContainer
    {
         public static HtmlTable Table(this HtmlHelper helper)
        {
            return new HtmlTable();
        }
    }
}