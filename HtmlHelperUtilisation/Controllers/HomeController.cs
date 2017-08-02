using System.Collections.Generic;
using System.Web.Mvc;
using HtmlHelperUtilisation.Models;

namespace HtmlHelperUtilisation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new TestViewModel
            {
                ListTest = new List<RowViewModel>
                {
                    new RowViewModel
                    {
                        Col1 = "val lololo 10",
                        Col2 = "val 20",
                        Col3 = "val lololo 30",
                    },
                    new RowViewModel
                    {
                        Col1 = "val test 11",
                        Col2 = "val test 21",
                        Col3 = "val lololo 31",
                    },
                    new RowViewModel
                    {
                        Col1 = "val patate 12",
                        Col2 = "val test 22",
                        Col3 = "val lololo 32",
                    },
                    new RowViewModel
                    {
                        Col1 = "val lololo 13",
                        Col2 = "val patate 23",
                        Col3 = "val test 33",
                    }
                }
            };

            return View(model);
        }
    }
}