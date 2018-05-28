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
    public class BookmarksController : AServiceController
    {
        // GET: Bookmarks/New
        [HttpGet]
        public async Task<ActionResult> New(Guid collectionId)
        {
            var user = await WhoAmI();
            BookmarkViewModel bm = new BookmarkViewModel();
            bm.CollectionId = collectionId;
            return View(bm);
        }

        // POST: Bookmarks/New
        [HttpPost]
        public async Task<ActionResult> New(BookmarkViewModel bookmark)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CollectionDetails", "Collections", new { id = bookmark.CollectionId });
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "Bookmarks");
            apiRequest.Content = new ObjectContent<BookmarkViewModel>(bookmark, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("CollectionDetails", "Collections", new { id = bookmark.CollectionId });
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CollectionDetails", "Collections", new { id = bookmark.CollectionId });
            }

            PassCookiesToClient(apiResponse);
            return RedirectToAction("CollectionDetails", "Collections", new { id = bookmark.CollectionId });
        }

        // GET: Bookmarks/Details/{id}
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Bookmarks/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("PublicCollections", "Collections");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("PublicCollections", "Collections");
            }

            BookmarkViewModel bm = await apiResponse.Content.ReadAsAsync<BookmarkViewModel>();
            if(bm.Collection == null)
            {
                await bm.InitCollectionAsync(bm.CollectionId);
            }

            PassCookiesToClient(apiResponse);
            var user = await WhoAmI();
            
            if(bm.Collection.Private && (user == null || bm.Collection.OwnerId != user.Id))
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            if(user == null)
            {
                return View(bm);
            }

            if(bm.Collection.OwnerId == user.Id)
            {
                return View("MyDetails", bm);
            }
            else
            {
                return View(bm);
            }
        }

        // GET: Bookmarks/{id}/Edit
        [HttpGet]
        [Route("Bookmarks/{id}/Edit")]
        public async Task<ActionResult> Edit(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Bookmarks/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("Details", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", id);
            }

            BookmarkViewModel bm = await apiResponse.Content.ReadAsAsync<BookmarkViewModel>();

            PassCookiesToClient(apiResponse);

            var user = await WhoAmI();
            if(bm.Collection.OwnerId != user.Id)
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            return View(bm);
        }

        // POST: Bookmarks/{id}/Edit
        [HttpPost]
        [Route("Bookmarks/{id}/Edit")]
        public async Task<ActionResult> Edit(BookmarkViewModel bm)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Put, "Bookmarks");
            apiRequest.Content = new ObjectContent<BookmarkViewModel>(bm, new JsonMediaTypeFormatter());

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("Details", new { id = bm.Id });
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = bm.Id });
            }


            PassCookiesToClient(apiResponse);
            return RedirectToAction("Details", new { id = bm.Id });
        }

        // GET: Bookmarks/{id}/Delete
        [HttpGet]
        [Route("Bookmarks/{id}/Delete")]
        public async Task<ActionResult> Delete(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Bookmarks/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("Details", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", id);
            }

            BookmarkViewModel bm = await apiResponse.Content.ReadAsAsync<BookmarkViewModel>();

            PassCookiesToClient(apiResponse);

            var user = await WhoAmI();
            if(bm.Collection.OwnerId != user.Id)
            {
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }

            return View(bm);
        }

        // DELETE: Bookmarks/{id}/Delete
        [HttpPost]
        [Route("Bookmarks/{id}/Delete")]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(Guid id)
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Delete, $"Bookmarks/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return RedirectToAction("Details", id);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", id);
            }


            PassCookiesToClient(apiResponse);

            return RedirectToAction("MyCollections", "Collections");
        }





    }
}