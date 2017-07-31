using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.HtmlControls;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;

namespace HtmlTableHelper
{
    public static class TableHelpers
    {
        public static HtmlTable<TValue> DisplayTable<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression) where TValue : IEnumerable
        {
            Func<TModel, TValue> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);

            return new HtmlTable<TValue>(result);
        }
    }
}