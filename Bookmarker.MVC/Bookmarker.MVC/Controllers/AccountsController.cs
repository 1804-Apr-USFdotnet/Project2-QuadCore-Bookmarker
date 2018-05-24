using Bookmarker.MVC.Models;
using NLog;
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
        // GET: Accounts/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Accounts/Register
        public ActionResult Register()
        {
            return View();
        }

        private async Task<bool> Create(AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            UserAPI user = new UserAPI();
            user.Username = account.Username;
            user.Email = account.Email;

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Users");
            apiRequest.Content = new ObjectContent<UserAPI>(user, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return false;
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        // POST: Accounts/Register
        [HttpPost]
        public async Task<ActionResult> Register(AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            // Create User in BookmarkerDb
            if(!(await Create(account)))
            {
                return View("Error");
            }

            // Create Account in BookmarkerAccountDb
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Accounts/Register");
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

            return RedirectToAction("Login", "Accounts", account);
        }

        // POST: Accounts/Login
        [HttpPost]
        public async Task<ActionResult> Login(AccountViewModel account)
        {
            Logger logger = LogManager.GetLogger("file");
            logger.Log(LogLevel.Info, "START");

            if (!ModelState.IsValid)
            {
                logger.Log(LogLevel.Info, "Model state invalid");
                return View();
            }

            logger.Log(LogLevel.Info, "Create API request");
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Accounts/Login");
            logger.Log(LogLevel.Info, "assign API Request content");
            apiRequest.Content = new ObjectContent<AccountViewModel>(account, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                logger.Log(LogLevel.Info, "send API request");
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch(Exception e)
            {
                logger.Log(LogLevel.Info, $"Error sending API request: {e}");
                return View();
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                logger.Log(LogLevel.Info, $"Status Code: {apiResponse.StatusCode}");
                return View();
            }

            logger.Log(LogLevel.Info, $"Pass cookies and redirect");
            PassCookiesToClient(apiResponse);

            return RedirectToAction("Home", "Home");
        }

        // GET: Accounts/Logout
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Unable to logout";
                return RedirectToAction("Home", "Home");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Accounts/Logout");

            HttpResponseMessage apiResponse;

            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                TempData["Message"] = "Unable to logout";
                return RedirectToAction("Home", "Home");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                TempData["Message"] = "Unable to logout";
                return RedirectToAction("Home", "Home");
            }

            PassCookiesToClient(apiResponse);

            TempData["Message"] = "Logged out.";
            return RedirectToAction("Home", "Home");
        }

    }
}