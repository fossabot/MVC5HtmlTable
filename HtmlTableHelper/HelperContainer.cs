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
        public static HtmlTable<TRowModel> DisplayTable<TRowModel>(this HtmlHelper helper)
        {
            return new HtmlTable<TRowModel>(helper.ViewData.Model);
        }

        public static HtmlTable<TRowModel> DisplayTable<TRowModel, TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            return new HtmlTable<TRowModel>(helper.ViewData.Model);
        }

        //public static HtmlTable<TRowModel> DisplayTable<TData>(this HtmlHelper<TData> helper, Expression<Func<TData, TRowModel>> expression) where TData : IEnumerable
        //{
        //    Func<TData, TRowModel> deleg = expression.Compile();
        //    var result = deleg(helper.ViewData.Model);

        //    return new HtmlTable<TRowModel>(result);
        //}
    }
}