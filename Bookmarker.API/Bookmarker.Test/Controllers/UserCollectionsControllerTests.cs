using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookmarker.API.Controllers;
using Bookmarker.API.Models;
using Bookmarker.Models;
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

        private readonly string userGuid1 = "88888888-4444-4444-4444-222222222222";
        private readonly string userGuid2 = "11111111-4444-4444-4444-222222222222";
        private readonly string userGuidNotExist = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

        public UserCollectionsControllerTests()
        {
            controller = new UserCollectionsController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
        }

        [TestMethod()]
        public async Task GetTest()
        {
            // Arrange
            Guid userId = new Guid(userGuid1);
            string expectedCollectionName = "c#";
            string expectedDescription = ".net framework";
            // Act
            IHttpActionResult result = controller.Get(userId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<IEnumerable<CollectionAPI>>();

            int actualCount = collection.Count();
            string actualName = collection.FirstOrDefault().Name;
            string actualDescription = collection.FirstOrDefault().Description;

            // Assert
            Assert.AreEqual(1, actualCount);
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod()]
        public async Task GetBadRequestTest()
        {
            // check, if userId is not in database
            // returns BadRequest HttpActionResult
            Guid userId = new Guid(userGuidNotExist);
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;

            IHttpActionResult result = controller.Get(userId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod()]
        public async Task GetCollectionByIndexTest()
        {
            // Arrange
            Guid userId = new Guid(userGuid2);
            int index = 2;
            string expectedCollectionName = "c#";
            string expectedDescription = "c# tutorials";

            // Act
            IHttpActionResult result = controller.GetCollectionByIndex(userId, index);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<CollectionAPI>();

            string actualName = collection.Name;
            string actualDescription = collection.Description;

            // Assert
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod()]
        public async Task GetCollectionByIndexBadRequestTest()
        {
            // check, if userId is not in database
            // returns BadRequest HttpActionResult
            Guid userId = new Guid(userGuidNotExist);
            int index = 1;
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;
            

            IHttpActionResult result = controller.GetCollectionByIndex(userId, index);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod()]
        public async Task GetCollectionByIndexNotFoundTest()
        {
            // check, if index is greater than collection count
            // returns NotFound HttpActionResult
            Guid userId = new Guid(userGuid2);
            int index = 3;
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;

            IHttpActionResult result = controller.GetCollectionByIndex(userId, index);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod()]
        public async Task GetCollectionByIdTest()
        {
            // Arrange
            Guid userId = new       Guid(userGuid2);
            Guid collectionId = new Guid("bbbbbbbb-4444-4444-4444-222222222222");
            string expectedCollectionName = "recipes";
            string expectedDescription = "my favorites";
            // Act
            IHttpActionResult result = controller.GetCollectionById(userId, collectionId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await response.Content.ReadAsAsync<CollectionAPI>();

            string actualName = collection.Name;
            string actualDescription = collection.Description;

            // Assert
            Assert.AreEqual(expectedCollectionName, actualName);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod()]
        public async Task GetCollectionByIdBadRequestTest()
        {
            // check, if userId is not in database
            // returns BadRequest HttpActionResult
            Guid userId = new Guid(userGuidNotExist);
            Guid collectionId = new Guid("bbbbbbbb-4444-4444-4444-222222222222");
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;

            IHttpActionResult result = controller.GetCollectionById(userId, collectionId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod()]
        public async Task GetCollectionByIdNotFoundTest()
        {
            // check, if collectionId is non-existent for user
            // returns NotFound HttpActionResult
            Guid userId = new Guid(userGuid2);
            Guid collectionId = new Guid("eeeeeeee-4444-0000-4444-222222222222");
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;

            IHttpActionResult result = controller.GetCollectionById(userId, collectionId);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }
    }
}