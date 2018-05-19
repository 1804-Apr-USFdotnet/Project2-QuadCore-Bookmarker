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
    public class TestCollectionsApiController
    {
        private readonly CollectionsController controller;

        public TestCollectionsApiController()
        {
            controller = new CollectionsController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task TestCollectionsApiGetAsync()
        {
            // Arrange
            int expectedCollectionCount = 3;
            int actualCollectionCount = 0;

            // Act
            IHttpActionResult collectionResult = controller.Get();
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            // Assert
            Assert.AreEqual(expectedCollectionCount, actualCollectionCount);
        }

        [TestMethod]
        public async Task TestCollectionsApiGetByIdAsync()
        {
            // Arrange
            string expectedCollectionname = "smith";
            Guid smithsGuid = new Guid("88888888-4444-4444-4444-222222222222");

            // Act
            IHttpActionResult collectionResult = controller.Get(smithsGuid);
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collection = await message.Content.ReadAsAsync<Collection>();

            // Assert
            Assert.AreEqual(expectedCollectionname, collection.Name);
        }

        [TestMethod]
        public async Task TestCollectionsApiPostAsync()
        {
            // Arrange
            int actualCollectionCount = 0;

            // Act
            IHttpActionResult collectionResult = controller.Get();
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            //////////////////////////////////////////////////////////////////

            // Post -- invalid model -- expect bad request message
            Collection newCollection = new Collection();
            newCollection.Name = "jon";
            newCollection.Description = "badPw";
            newCollection.Id = new Guid("55555555-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("k1", "password is too short");
            IHttpActionResult result = controller.Post(newCollection);
            var badPostMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badPostMessage.StatusCode);

            newCollection.Description = "okayPassword";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Post(newCollection);
            var goodPostMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodPostMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Arrange
            int expectedCollectionCount = actualCollectionCount + 1;
            // reset count to test again after the POST
            actualCollectionCount = 0;

            // Act
            collectionResult = controller.Get();
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            // Assert
            Assert.AreEqual(expectedCollectionCount, actualCollectionCount);
        }

        [TestMethod]
        public async Task TestCollectionsApiPutAsync()
        {
            // Find out how many collections there are

            // Arrange
            int actualCollectionCount = 0;

            // Act
            IHttpActionResult collectionResult = controller.Get();
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            //////////////////////////////////////////////////////////////////

            // PUT a new collection with invalid model - expect bad request message
            Collection newCollection = new Collection();
            newCollection.Name = "jon";
            newCollection.Description = "badPw";
            newCollection.Id = new Guid("55555555-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("k1", "password is too short");
            IHttpActionResult result = controller.Put(newCollection);
            var badMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badMessage.StatusCode);

            // Make new collection's model valid and PUT
            newCollection.Description = "okayPassword";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Put(newCollection);
            var goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Expect collection count to be one more than before

            // Arrange
            int expectedCollectionCount = actualCollectionCount + 1;
            // reset count to test again after the PUT
            actualCollectionCount = 0;

            // Act
            collectionResult = controller.Get();
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            // Assert
            Assert.AreEqual(expectedCollectionCount, actualCollectionCount);

            /////////////////////////////////////////////////////////////////

            // Now do an update to existing collection with PUT
            // collection count shouldn't change

            // Arrange
            expectedCollectionCount = actualCollectionCount;
            // reset count to test again after the PUT
            actualCollectionCount = 0;

            Collection oldCollection = new Collection();
            oldCollection.Name = "jon";
            oldCollection.Description = "newPassword";
            oldCollection.Id = new Guid("55555555-4444-4444-4444-222222222222");

            goodResult = controller.Put(oldCollection);
            goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            // Act
            collectionResult = controller.Get();
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var collection in collections)
            {
                actualCollectionCount++;
            }

            // Assert
            Assert.AreEqual(expectedCollectionCount, actualCollectionCount);
        }

        [TestMethod]
        public async Task TestCollectionsApiDeleteAsync()
        {
            // Find out how many collections there are

            // Arrange
            int actualCollectionCount = 0;

            // Act
            IHttpActionResult collectionResult = controller.Get();
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var u in collections)
            {
                actualCollectionCount++;
            }
            
            ///////////////////////////////////////////////////////////////

            // Arrange
            Guid franksGuid = new Guid("11111111-4444-4444-4444-222222222222");
            Guid wrongGuid = new Guid("99999999-1111-4444-4444-222222222222");

            // Act
            collectionResult = controller.Delete(wrongGuid);
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, message.StatusCode);

            // Act
            collectionResult = controller.Delete(franksGuid);
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);

            ///////////////////////////////////////////////////////////////

            // Check that there is one less collection

            // Arrange
            int expectedCollectionCount = actualCollectionCount - 1;
            actualCollectionCount = 0;

            // Act
            collectionResult = controller.Get();
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            foreach (var u in collections)
            {
                actualCollectionCount++;
            }

            Assert.AreEqual(expectedCollectionCount, actualCollectionCount);
        }
    }
}
