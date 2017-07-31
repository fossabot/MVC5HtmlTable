using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlHelperUtilisation.Models;

namespace HtmlHelperUtilisation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new TestViewModel()
            {
                ListTest = new List<RowViewModel>
                {
                    new RowViewModel(),
                    new RowViewModel(),
                    new RowViewModel(),
                    new RowViewModel()
                }
            };

            return View(model);
        }
    }
}