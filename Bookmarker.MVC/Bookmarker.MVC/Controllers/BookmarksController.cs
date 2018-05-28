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

    }
}