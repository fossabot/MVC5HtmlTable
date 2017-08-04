using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Library.Logic;

namespace Library
{
    public static class TableHelpers
    {
        public static HtmlTable<TRowModel> DisplayTable<TModel, TRowModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TRowModel>>> expression)
        {

            Func<TModel, IEnumerable<TRowModel>> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);

            return new HtmlTable<TRowModel>(helper.ViewData.Model, result, helper.ViewContext.Writer);
        }

        public static HtmlTable<TRowModel> DisplayTable<TModel, TRowModel>(this HtmlHelper<TModel> helper, IEnumerable<TRowModel> data)
        {
            return new HtmlTable<TRowModel>(helper.ViewData.Model, data, helper.ViewContext.Writer);
        }

        public static HtmlTable<TRowModel> DisplayTableForModel<TRowModel>(this HtmlHelper<IEnumerable<TRowModel>> helper)
        {
            return helper.DisplayTable(helper.ViewData.Model);
        }

        // Have to re-define the IEnumerable otherwsie we get a compile-time error "List can't be cast to IEnumerable" -_-
        public static HtmlTable<TRowModel> DisplayTableForModel<TRowModel>(this HtmlHelper<List<TRowModel>> helper)
        {
            return helper.DisplayTable(helper.ViewData.Model);
        }
    }
}