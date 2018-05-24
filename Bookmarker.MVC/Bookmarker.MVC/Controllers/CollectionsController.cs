using Bookmarker.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bookmarker.MVC.Controllers
{
    public class CollectionsController : AServiceController
    {
        // GET: Public Collections
        public async Task<ActionResult> PublicCollections()
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

            return View(await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>());
        }

        public async Task<UserAPI> WhoAmI()
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Accounts/WhoAmI");
            HttpResponseMessage apiResponse;
            apiResponse = await HttpClient.SendAsync(apiRequest);
            PassCookiesToClient(apiResponse);
            var user = await apiResponse.Content.ReadAsAsync<UserAPI>();
            return user;
        }

        // GET: Public Collections
        public async Task<ActionResult> MyCollections()
        {
            Guid? id = null;
            try
            {
                var user = await WhoAmI();
                id = user?.Id;
                if(id == null)
                {
                    TempData["Message"] = "Please log in.";
                    return RedirectToAction("Login", "Accounts");
                }
            }
            catch
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"users/{id}/collections");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            return View(await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>());
        }
    }
}