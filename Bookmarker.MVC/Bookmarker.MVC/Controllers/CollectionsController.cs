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
        public async Task<ActionResult> PublicCollections(string search)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Collections?search=" + search);

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            var collections = await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>();

            foreach (var collection in collections)
            {
                await collection.InitBookmarksAsync();
            }
            return View("CollectionList", collections);
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

            PassCookiesToClient(apiResponse);

            return View("CollectionList", await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>());
        }
    }
}