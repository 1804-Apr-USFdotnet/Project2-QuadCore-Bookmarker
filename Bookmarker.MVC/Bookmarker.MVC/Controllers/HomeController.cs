using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bookmarker.MVC.Models;
using NLog;

namespace Bookmarker.MVC.Controllers
{
    public class HomeController : AServiceController
    {
        public async Task<ActionResult> GuestLanding()
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Collections");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            return RedirectToAction("PublicCollections", "Collections");
        }

        public async Task<ActionResult> Home()
        {
            try
            {
                var user = await WhoAmI();
                if(user==null)
                {
                    return RedirectToAction("GuestLanding", "Home");
                }
                return RedirectToAction("MyCollections", "Collections");
            }
            catch
            {
                return RedirectToAction("GuestLanding", "Home");
            }
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
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                return View(await apiResponse.Content.ReadAsAsync<IEnumerable<AccountViewModel>>());
            }
        }
    }
}