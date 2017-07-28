using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RazorEngine;
using RazorEngine.Templating;

namespace HtmlTableHelper
{
    public static class TestClass
    {
        public static string Testhelper(this HtmlHelper helper)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Engine.Razor.RunCompile("Hello @Model.Name", "templateKey", null, new { Name = "test" }));
            return builder.ToString();
        }
    }
}
