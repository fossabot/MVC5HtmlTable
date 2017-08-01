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