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
        public static HtmlTable<TRowModel> DisplayTable<TModel, TRowModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TRowModel>>> expression)
        {

            Func<TModel, IEnumerable<TRowModel>> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);

            return new HtmlTable<TRowModel>(helper.ViewData.Model, result);
        }
    }
}