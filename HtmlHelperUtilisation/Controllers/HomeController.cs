using System.Collections.Generic;
using System.Web.Mvc;
using HtmlHelperUtilisation.Models;

namespace HtmlHelperUtilisation.Controllers
{
    public class HomeController : Controller
    {
        private List<RowViewModel> _getTestData => new List<RowViewModel>
                {
                    new RowViewModel
                    {
                        Col1 = "val lololo 10",
                        Col2 = "val 20",
                        Col3 = "val lololo 30",
                        Col4 = 10,
                        Col5 = true
                    },
                    new RowViewModel
                    {
                        Col1 = "val test 11",
                        Col2 = "val test 21",
                        Col3 = "val lololo 31",
                        Col4 = 69696599
                    },
                    new RowViewModel
                    {
                        Col1 = "val patate 12",
                        Col2 = "val test 22",
                        Col3 = "val lololo 32",
                        Col4 = 1501,
                        Col5 = true
                    },
                    new RowViewModel
                    {
                        Col1 = "val lololo 13",
                        Col2 = "val patate 23",
                        Col3 = "val test 33",
                        Col4 = 100,
                        Col5 = true
                    }
                };

        public ActionResult Index()
        {
            return View(new TestViewModel
            {
                ListTest = _getTestData
            });
        }

        public ActionResult Test()
        {
            return View(_getTestData);
        }
    }
}