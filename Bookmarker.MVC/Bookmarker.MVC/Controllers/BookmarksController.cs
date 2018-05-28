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
    }
}