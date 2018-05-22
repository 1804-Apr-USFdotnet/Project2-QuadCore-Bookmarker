using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Bookmarker.API.Controllers;
using Bookmarker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bookmarker.Test
{
    [TestClass]
    public class TestBookmarksApiController
    {
        private readonly BookmarksController controller;

        public TestBookmarksApiController()
        {
            controller = new BookmarksController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task TestBookmarksApiGetAsync()
        {
            // Arrange
            int expectedBookmarkCount = 4;
            int actualBookmarkCount = 0;

            // Act
            IHttpActionResult bookmarkResult = controller.Get();
            var message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }

            // Assert
            Assert.AreEqual(expectedBookmarkCount, actualBookmarkCount);
        }

        [TestMethod]
        public async Task TestBookmarksApiGetByIdAsync()
        {
            // Arrange
            string expectedBookmarkName = "c# keywords";
            Guid bookmarkGuid = new Guid("00000000-4444-4444-4444-222222222222");

            // Act
            IHttpActionResult bookmarkResult = controller.Get(bookmarkGuid);
            var message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmark = await message.Content.ReadAsAsync<Bookmark>();

            // Assert
            Assert.AreEqual(expectedBookmarkName, bookmark.Name);
        }

        [TestMethod]
        public async Task TestBookmarksApiPostAsync()
        {
            // Arrange
            int actualBookmarkCount = 0;

            // Act
            IHttpActionResult bookmarkResult = controller.Get();
            var message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }
            
            //////////////////////////////////////////////////////////////////
            
            Bookmark newBookmark = new Bookmark();
            newBookmark.Name = "Youtube";
            newBookmark.URL = null;
            newBookmark.CollectionId = new Guid("aaaaaaaa-4444-4444-4444-222222222222");
            newBookmark.Id = new Guid("09999999-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("urlreq", "URL is required");
            IHttpActionResult result = controller.Post(new API.Models.BookmarkAPI(newBookmark));
            var badPostMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badPostMessage.StatusCode);

            newBookmark.URL = "https://youtube.com/";
            controller.ModelState.Remove("urlreq");
            IHttpActionResult goodResult = controller.Post(new API.Models.BookmarkAPI(newBookmark));
            var goodPostMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodPostMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Arrange
            int expectedBookmarkCount = actualBookmarkCount + 1;
            // reset count to test again after the POST
            actualBookmarkCount = 0;

            // Act
            bookmarkResult = controller.Get();
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }

            // Assert
            Assert.AreEqual(expectedBookmarkCount, actualBookmarkCount);
        }

        [TestMethod]
        public async Task TestBookmarksApiPutAsync()
        {
            // Find out how many bookmarks there are

            // Arrange
            int actualBookmarkCount = 0;

            // Act
            IHttpActionResult bookmarkResult = controller.Get();
            var message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }

            //////////////////////////////////////////////////////////////////

            // PUT a new bookmark with invalid model - expect bad request message
            Bookmark newBookmark = new Bookmark();
            newBookmark.Name = "Google";
            newBookmark.URL = null;
            newBookmark.CollectionId = new Guid("bbbbbbbb-4444-4444-4444-222222222222");
            newBookmark.Id = new Guid("05555555-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("urlreq", "URL is required");
            IHttpActionResult result = controller.Put(new API.Models.BookmarkAPI(newBookmark));
            var badMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badMessage.StatusCode);

            // Make new bookmark's model valid and PUT
            newBookmark.URL = "Https://google.com/";
            controller.ModelState.Remove("urlreq");
            IHttpActionResult goodResult = controller.Put(new API.Models.BookmarkAPI(newBookmark));
            var goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Expect bookmark count to be one more than before

            // Arrange
            int expectedBookmarkCount = actualBookmarkCount + 1;
            // reset count to test again after the PUT
            actualBookmarkCount = 0;

            // Act
            bookmarkResult = controller.Get();
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }

            // Assert
            Assert.AreEqual(expectedBookmarkCount, actualBookmarkCount);

            /////////////////////////////////////////////////////////////////

            // Now do an update to existing bookmark with PUT
            // bookmark count shouldn't change

            // Arrange
            expectedBookmarkCount = actualBookmarkCount;
            // reset count to test again after the PUT
            actualBookmarkCount = 0;

            Bookmark oldBookmark = new Bookmark();
            oldBookmark.Name = "Google";
            oldBookmark.URL = "Https://google.com/";
            oldBookmark.CollectionId = new Guid("bbbbbbbb-4444-4444-4444-222222222222");
            oldBookmark.Id = new Guid("05555555-4444-4444-4444-222222222222");

            goodResult = controller.Put(new API.Models.BookmarkAPI(newBookmark));
            goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            // Act
            bookmarkResult = controller.Get();
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var bookmark in bookmarks)
            {
                actualBookmarkCount++;
            }

            // Assert
            Assert.AreEqual(expectedBookmarkCount, actualBookmarkCount);
        }

        [TestMethod]
        public async Task TestBookmarksApiDeleteAsync()
        {
            // Find out how many bookmarks there are

            // Arrange
            int actualBookmarkCount = 0;

            // Act
            IHttpActionResult bookmarkResult = controller.Get();
            var message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var b in bookmarks)
            {
                actualBookmarkCount++;
            }

            ///////////////////////////////////////////////////////////////

            // Arrange
            Guid b1Guid = new Guid("ffffffff-4444-4444-4444-222222222222");
            Guid wrongGuid = new Guid("99999999-1111-4444-4444-222222222222");

            // Act
            bookmarkResult = controller.Delete(wrongGuid);
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, message.StatusCode);

            // Act
            bookmarkResult = controller.Delete(b1Guid);
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);

            ///////////////////////////////////////////////////////////////

            // Check that there is one less Bookmark

            // Arrange
            int expectedBookmarkCount = actualBookmarkCount - 1;
            actualBookmarkCount = 0;

            // Act
            bookmarkResult = controller.Get();
            message = await bookmarkResult.ExecuteAsync(new System.Threading.CancellationToken());
            bookmarks = await message.Content.ReadAsAsync<IEnumerable<Bookmark>>();
            foreach (var b in bookmarks)
            {
                actualBookmarkCount++;
            }

            Assert.AreEqual(expectedBookmarkCount, actualBookmarkCount);
        }
    }
}
