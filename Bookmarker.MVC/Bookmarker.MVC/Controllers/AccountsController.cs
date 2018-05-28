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
        public async Task<ActionResult> Login()
        {
            UserAPI user = await WhoAmI();
            if(user != null)
            {
                TempData["Message"] = "Already logged in.";
                return RedirectToAction("UserDetails", "Accounts");
            }
            return View();
        }

        // GET: Accounts/Register
        public async Task<ActionResult> Register()
        {
            UserAPI user = await WhoAmI();
            if(user != null)
            {
                TempData["Message"] = "Do you really want another account?";
            }
            return View();
        }

        // GET: Accounts/UserDetails
        public async Task<ActionResult> UserDetails()
        {
            UserAPI user = await WhoAmI();
            if(user == null)
            { 
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }
            return View(user);
        }

        // GET: Accounts/EditUser
        public async Task<ActionResult> EditUser()
        {
            UserAPI user = await WhoAmI();
            if(user == null)
            { 
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            AccountEdit editUser = new AccountEdit();
            editUser.Username = user.Username;
            editUser.newUsername = user.Username;
            editUser.Email = user.Email;
            editUser.newEmail = user.Email;
            editUser.Created = user.Created;
            editUser.Modified = user.Modified;
            editUser.Id = user.Id;

            return View(editUser);
        }

        //POST: Accounts/EditUser
        [HttpPost]
        public async Task<ActionResult> EditUser(AccountEdit user)
        {
            UserAPI apiUser = new UserAPI();
            apiUser.Username = user.newUsername ?? user.Username;
            apiUser.Email = user.newEmail;
            apiUser.Created = user.Created;
            apiUser.Modified = user.Modified;
            apiUser.Id = user.Id;

            // Edit User
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Put, "Users");
            apiRequest.Content = new ObjectContent<UserAPI>(apiUser, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("UserDetails");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("UserDetails");
            }

            PassCookiesToClient(apiResponse);


            // Edit Identity User
            apiRequest = CreateRequestToService(HttpMethod.Put, "Accounts/Edit");
            apiRequest.Content = new ObjectContent<AccountEdit>(user, new JsonMediaTypeFormatter());

            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                // TODO: Rollback User edit
                return RedirectToAction("UserDetails");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("UserDetails");
            }

            PassCookiesToClient(apiResponse);


            return RedirectToAction("UserDetails");
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

            PassCookiesToClient(apiResponse);
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

            return RedirectToAction("UserDetails");
        }

        // POST: Accounts/Login
        [HttpPost]
        public async Task<ActionResult> Login(AccountViewModel account)
        {
            Logger logger = LogManager.GetLogger("file");

            if (!ModelState.IsValid)
            {
                logger.Log(LogLevel.Info, "Model state invalid");
                return View();
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Accounts/Login");
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

            PassCookiesToClient(apiResponse);

            return RedirectToAction("UserDetails", "Accounts");
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

            PassCookiesToClient(apiResponse);

            if (!apiResponse.IsSuccessStatusCode)
            {
                TempData["Message"] = "Unable to logout";
                return RedirectToAction("Home", "Home");
            }

            TempData["Message"] = "Logged out.";
            return RedirectToAction("Home", "Home");
        }

    }
}