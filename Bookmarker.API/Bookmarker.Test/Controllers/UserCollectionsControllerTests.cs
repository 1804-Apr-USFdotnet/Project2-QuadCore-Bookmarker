using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookmarker.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bookmarker.Test;
using System.Net.Http;

namespace Bookmarker.API.Controllers.Tests
{
    [TestClass()]
    public class UserCollectionsControllerTests
    {
        private readonly UserCollectionsController controller;

        //[TestMethod()]
        public UserCollectionsControllerTests()
        {
            controller = new UserCollectionsController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
        }

        //[TestMethod()]
        //public void UserCollectionsControllerTest1()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public async Task GetTest()
        {
            // Arrange
            Guid userId = new Guid("88888888-4444-4444-4444-222222222222");
            string expectedCollectionName = "c#";
            string expectedDescription = ".net framework";
            // Act
            IHttpActionResult result = controller.Get(userId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<IEnumerable<Models.Collection>>();

            int actualCount = collection.Count();
            string actualName = collection.FirstOrDefault().Name;
            string actualDescription = collection.FirstOrDefault().Description;

            // Assert
            Assert.AreEqual(1, actualCount);
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod()]
        public async Task GetCollectionByIndexTest()
        {
            // Arrange
            Guid userId = new Guid("11111111-4444-4444-4444-222222222222");
            string expectedCollectionName = "c#";
            string expectedDescription = "c# tutorials";
            // Act
            IHttpActionResult result = controller.GetCollectionByIndex(userId, 2);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<Models.Collection>();

            string actualName = collection.Name;
            string actualDescription = collection.Description;

            // Assert
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod()]
        public async Task GetCollectionByIdTest()
        {
            // Arrange
            Guid userId = new       Guid("11111111-4444-4444-4444-222222222222");
            Guid collectionId = new Guid("bbbbbbbb-4444-4444-4444-222222222222");
            string expectedCollectionName = "recipes";
            string expectedDescription = "my favorites";
            // Act
            IHttpActionResult result = controller.GetCollectionById(userId, collectionId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<Models.Collection>();

            string actualName = collection.Name;
            string actualDescription = collection.Description;

            // Assert
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }
    }
}