using Bookmarker.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bookmarker.MVC.Controllers
{
    public class AccountsController : AServiceController
    {
        public async Task<bool> LoggedIn()
        {
            HttpRequestMessage apiRequest = 
                CreateRequestToService(HttpMethod.Get, "Accounts/LoggedIn");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
                if(apiResponse.Content.ToString() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // GET: Accounts/Login
        public ActionResult Login()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        // GET: Accounts/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        [HttpPost]
        public async Task<ActionResult> Register(AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            // TODO: Send register request to API

            return RedirectToAction("Home", "Home");
        }

        // POST: Accounts/Login
        [HttpPost]
        public async Task<ActionResult> Login(AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Accounts/Login");
            apiRequest.Content = new ObjectContent<AccountViewModel>(account, new JsonMediaTypeFormatter());

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
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            return RedirectToAction("Home", "Home");
        }

        // GET: Accounts/Logout
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Accounts/Logout");

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
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            return RedirectToAction("Home", "Home");
        }

        private bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if (apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                foreach (string value in values)
                {
                    Response.Headers.Add("Set-Cookie", value);
                }
                return true;
            }
            return false;
        }
    }
}