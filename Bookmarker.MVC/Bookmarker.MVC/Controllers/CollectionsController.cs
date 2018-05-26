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
    public class CollectionsController : AServiceController
    {
        // GET: Collections/PublicCollections
        public async Task<ActionResult> PublicCollections(string search, string sort = "name")
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Collections?search=" + search + "&sort=" + sort);

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

        // GET: Collections/MyCollections
        public async Task<ActionResult> MyCollections(string search, string sort = "name")
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

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"users/{id}/collections?search=" + search + "&sort=" + sort);

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

            return View(await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>());
        }


        // GET: Collections/AddCollection
        [HttpGet]
        public async Task<ActionResult> AddCollection()
        {
            var user = await WhoAmI();
            CollectionViewModel collection = new CollectionViewModel();
            collection.Owner = user.Id;
            return View(collection);
        }

        // POST: Collections/AddCollection
        [HttpPost]
        public async Task<ActionResult> AddCollection(CollectionViewModel collection)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("MyCollections");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Collections");
            apiRequest.Content = new ObjectContent<CollectionViewModel>(collection, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("MyCollections");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("MyCollections");
            }

            PassCookiesToClient(apiResponse);
            return RedirectToAction("MyCollections");
        }
    }
}