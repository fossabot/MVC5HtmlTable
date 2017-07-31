using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;

namespace HtmlTableHelper
{
    public static class TableHelpers
    {

        public static HtmlTable DisplayTable<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Func<TModel, TValue> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);
            //to do
            //return new MvcHtmlString(result.ToString());

            return new HtmlTable();
        }
    }
}