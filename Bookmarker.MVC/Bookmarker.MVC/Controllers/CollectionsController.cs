﻿using Bookmarker.MVC.Models;
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
        // GET: Collections/TopCollections
        //public async Task<ActionResult> TopCollections()
        //{
        //    HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Collections?sort=rating:desc");

        //    HttpResponseMessage apiResponse;
        //    try
        //    {
        //        apiResponse = await HttpClient.SendAsync(apiRequest);
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }

        //    PassCookiesToClient(apiResponse);

        //    var collections = await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>();

        //    collections = collections.Take(9);            

        //    foreach (var collection in collections)
        //    {
        //        await collection.InitBookmarksAsync();
        //    }
        //    var user = await WhoAmI();
        //    ViewBag.UserId = user?.Id;
        //    return View("Trending", collections);
        //}

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
            var user = await WhoAmI();
            ViewBag.UserId = user?.Id;
            return View("CollectionList", collections);
        }

        // GET: Collections/MyCollections
        public async Task<ActionResult> MyCollections(string search, string sort = "name")
        {
            Guid? id = null;
            UserAPI user = null;
            try
            {
                user = await WhoAmI();
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

            var collections = await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>();
            foreach (var collection in collections)
            {
                await collection.InitBookmarksAsync();
            }
            ViewBag.UserId = user?.Id;
            return View(collections);
        }


        // GET: Collections/AddCollection
        [HttpGet]
        public async Task<ActionResult> AddCollection()
        {
            var user = await WhoAmI();
            CollectionViewModel collection = new CollectionViewModel();
            collection.OwnerId = user.Id;
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

        // GET: Collections/{id}/Details
        [HttpGet]
        [Route("Collections/{id}/Details")]
        public async Task<ActionResult> CollectionDetails(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Collections/{id}");

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

            CollectionViewModel collection = await apiResponse.Content.ReadAsAsync<CollectionViewModel>();
            await collection.InitBookmarksAsync();

            PassCookiesToClient(apiResponse);

            var user = await WhoAmI();
            
            if(collection.Private && (user == null || collection.OwnerId != user.Id))
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            if(user == null)
            {
                return View(collection);
            }

            if(collection.OwnerId == user.Id)
            {
                return View("MyCollectionDetails", collection);
            }
            else
            {
                return View(collection);
            }
        }

        // GET: Collections/{id}/Edit
        [HttpGet]
        [Route("Collections/{id}/EditCollection")]
        public async Task<ActionResult> EditCollection(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Collections/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("CollectionDetails", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CollectionDetails", id);
            }

            CollectionViewModel collection = await apiResponse.Content.ReadAsAsync<CollectionViewModel>();

            PassCookiesToClient(apiResponse);

            var user = await WhoAmI();
            if(collection.OwnerId != user.Id)
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            return View(collection);
        }

        // POST: Collections/{id}/Edit
        [HttpPost]
        [Route("Collections/{id}/EditCollection")]
        public async Task<ActionResult> EditCollection(CollectionViewModel collection)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Put, "Collections");
            apiRequest.Content = new ObjectContent<CollectionViewModel>(collection, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("CollectionDetails", new { id = collection.Id });
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CollectionDetails", new { id = collection.Id });
            }


            PassCookiesToClient(apiResponse);

            return RedirectToAction("CollectionDetails", new { id = collection.Id });
        }

        // GET: Collections/{id}/Delete
        [HttpGet]
        [Route("Collections/{id}/Delete")]
        public async Task<ActionResult> DeleteCollection(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Collections/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("CollectionDetails", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CollectionDetails", id);
            }

            CollectionViewModel collection = await apiResponse.Content.ReadAsAsync<CollectionViewModel>();

            PassCookiesToClient(apiResponse);

            var user = await WhoAmI();
            if(collection.OwnerId != user.Id)
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            return View(collection);
        }

        // DELETE: Collections/{id}/Delete
        [HttpPost]
        [Route("Collections/{id}/Delete")]
        [ActionName("DeleteCollection")]
        public async Task<ActionResult> ConfirmDeleteCollection(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Delete, $"Collections/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("CollectionDetails", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CollectionDetails", id);
            }


            PassCookiesToClient(apiResponse);

            return RedirectToAction("MyCollections");
        }
    }
}