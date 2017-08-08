using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTable.Logic;

namespace HtmlTable
{
    /// <summary>
    /// Container for the HTML Helpers
    /// </summary>
    public static class TableHelpers
    {
        /// <summary>
        /// Creates a new <see cref="HtmlHelper{TModel}"/> from a property of the model, where the property is an <see cref="IEnumerable{TRowModel}"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TRowModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <returns>Instance of <see cref="HtmlTable{TRowModel}"/> - utilize as root of fluent API calls</returns>
        public static HtmlTable<TRowModel> DisplayTable<TModel, TRowModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TRowModel>>> expression)
        {
            Func<TModel, IEnumerable<TRowModel>> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);

            return new HtmlTable<TRowModel>(helper.ViewData.Model, result, helper.ViewContext.Writer);
        }

        /// <summary>
        /// Creates a new <see cref="HtmlHelper{TModel}"/> from an <see cref="IEnumerable{TRowModel}"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TRowModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="data"></param>
        /// <returns>Instance of <see cref="HtmlTable{TRowModel}"/> - utilize as root of fluent API calls</returns>
        public static HtmlTable<TRowModel> DisplayTable<TModel, TRowModel>(this HtmlHelper<TModel> helper, IEnumerable<TRowModel> data)
        {
            return new HtmlTable<TRowModel>(helper.ViewData.Model, data, helper.ViewContext.Writer);
        }

        /// <summary>
        /// Creates a new <see cref="HtmlHelper{TModel}"/> from a Model of type <see cref="IEnumerable{TRowModel}"/>
        /// </summary>
        /// <typeparam name="TRowModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns>Instance of <see cref="HtmlTable{TRowModel}"/> - utilize as root of fluent API calls</returns>
        public static HtmlTable<TRowModel> DisplayTableForModel<TRowModel>(this HtmlHelper<IEnumerable<TRowModel>> helper)
        {
            return helper.DisplayTable(helper.ViewData.Model);
        }

        // Have to re-define the IEnumerable otherwsie we get a compile-time error "List can't be cast to IEnumerable" -_-
        /// <summary>
        /// Creates a new <see cref="HtmlHelper{TModel}"/> from a Model of type <see cref="List{TRowModel}"/>
        /// </summary>
        /// <typeparam name="TRowModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns>Instance of <see cref="HtmlTable{TRowModel}"/> - utilize as root of fluent API calls</returns>
        public static HtmlTable<TRowModel> DisplayTableForModel<TRowModel>(this HtmlHelper<List<TRowModel>> helper)
        {
            return helper.DisplayTable(helper.ViewData.Model);
        }
    }
}