using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookmarker.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using Bookmarker.Test;

namespace Bookmarker.API.Controllers.Tests
{
    [TestClass()]
    public class CollectionBookmarksControllerTests
    {
        private readonly CollectionBookmarksController controller;

        private readonly string cId1 = "aaaaaaaa-4444-4444-4444-222222222222"; // 1 b
        private readonly string cId2 = "bbbbbbbb-4444-4444-4444-222222222222"; // 1 b
        private readonly string cId3 = "33333333-4444-4444-4444-222222222222"; // 2 b
        private readonly string cIdNotExist = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";
        private readonly string bId1 = "";
        private readonly string bId2 = "";
        private readonly string bIdNotExist = "";

        public CollectionBookmarksControllerTests()
        {
            controller = new CollectionBookmarksController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
        }

        [TestMethod()]
        public async Task GetTest()
        {
            Guid collection = new Guid(cId2);
            int expectedCount = 1;
            string expectedBookmarkName = "recipes";

            IHttpActionResult request = controller.Get(collection);
            var response = await request.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await response.Content.ReadAsAsync<IEnumerable<Models.BookmarkAPI>>();
            int actualCount = bookmarks.Count();
            string actualName = bookmarks.ElementAt(0).Name;

            Assert.AreEqual(expectedCount, bookmarks.Count());
            Assert.AreEqual(expectedBookmarkName, actualName);
        }

        [TestMethod()]
        public async Task GetByIndexTest()
        {
            Guid collection = new Guid(cId3);
            string expectedName = "c# keywords";
            string expectedUrl = "cskeywords.com";

            IHttpActionResult request = controller.GetByIndex(collection, 2);
            var response = await request.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmark = await response.Content.ReadAsAsync<Models.BookmarkAPI>();
            string actualName = bookmark.Name;
            string actualUrl = bookmark.URL;

            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [TestMethod()]
        public async Task GetByIdTest()
        {
            Guid collection = new Guid(cId3);
            Guid expectedBookmark = new Guid("cccccccc-4444-4444-4444-222222222222");
            string expectedName = "c# intro";
            string expectedUrl = "csharpintro.com";

            IHttpActionResult request = controller.GetById(collection, expectedBookmark);
            var response = await request.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmark = await response.Content.ReadAsAsync<Models.BookmarkAPI>();
            string actualName = bookmark.Name;
            string actualUrl = bookmark.URL;

            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedUrl, actualUrl);
        }
    }
}