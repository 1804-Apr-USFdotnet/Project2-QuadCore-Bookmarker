using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookmarker.MVC.Models;

namespace Bookmarker.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /* TODO: Check if user is logged in
             * If yes, send to user-specific home page
             * If no,  send to general home page
             */

            return View(new List<CollectionViewModel>());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}