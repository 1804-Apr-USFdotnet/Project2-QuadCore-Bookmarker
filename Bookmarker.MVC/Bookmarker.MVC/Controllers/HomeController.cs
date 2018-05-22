using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bookmarker.MVC.Models;

namespace Bookmarker.MVC.Controllers
{
    public class HomeController : AServiceController
    {
        public ActionResult Index()
        {
            /* TODO: Check if user is logged in
             * If yes, send to user-specific home page
             * If no,  send to general home page
             */

            return View(new List<CollectionViewModel>());
        }

        public ActionResult Home()
        {
            /* TODO: User-specific home page
             * if user is not authenticated, give them an error screen
             * otherwise, show their personal page
             */
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            AccountsController c = new AccountsController();
            ViewBag.Message = c.LoggedInAsync();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> UsersList()
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Users");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode != HttpStatusCode.Unauthorized)
                {
                    return View("Error");
                }
                TempData["Message"] = "Please log in";
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                return View(await apiResponse.Content.ReadAsAsync<IEnumerable<AccountViewModel>>());
            }

        }
    }
}