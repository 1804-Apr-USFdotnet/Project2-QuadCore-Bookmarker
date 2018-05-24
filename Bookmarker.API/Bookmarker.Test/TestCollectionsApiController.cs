using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Bookmarker.API.Controllers;
using Bookmarker.API.Models;
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
            string expectedCollectionname = "recipes";
            Guid guid = new Guid("bbbbbbbb-4444-4444-4444-222222222222");

            // Act
            IHttpActionResult collectionResult = controller.Get(guid);
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
            newCollection.Name = "";
            newCollection.Description = "The latest news!";
            newCollection.Id = new Guid("55555555-4444-aaaa-4444-222222222222");

            controller.ModelState.AddModelError("k1", "name is required");
            IHttpActionResult result = controller.Post(new CollectionAPI(newCollection));
            var badPostMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badPostMessage.StatusCode);

            newCollection.Name = "News";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Post(new CollectionAPI(newCollection));
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
            newCollection.Name = "";
            newCollection.Description = "Lots of things";
            newCollection.Id = new Guid("55555555-cccc-1111-4444-111111111111");

            controller.ModelState.AddModelError("k1", "Name is required");
            IHttpActionResult result = controller.Put(new CollectionAPI(newCollection));
            var badMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badMessage.StatusCode);

            // Make new collection's model valid and PUT
            newCollection.Name = "Things";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Put(new CollectionAPI(newCollection));
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
            oldCollection.Name = "Things";
            oldCollection.Description = "All the things.";
            oldCollection.Id = new Guid("55555555-cccc-1111-4444-111111111111");

            goodResult = controller.Put(new CollectionAPI(oldCollection));
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
            Guid wrongGuid = new Guid("11111111-4444-4444-4444-aaaaaaaaaaaa");
            Guid recipesGuid = new Guid("bbbbbbbb-4444-4444-4444-222222222222");

            // Act
            collectionResult = controller.Delete(wrongGuid);
            message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, message.StatusCode);

            // Act
            collectionResult = controller.Delete(recipesGuid);
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
        
        [TestMethod]
        public async Task TestCollectionsAPISortIntegration()
        {
            // Arrange
            string sort = "name:desc";

            // Act
            IHttpActionResult collectionResult = controller.Get(sort: sort);
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            var actualCollections = new List<Collection>(collections);

            var expectedCollections = new List<Collection>(actualCollections);
            Logic.Library.Sort(ref expectedCollections, sort);

            // Assert
            CollectionAssert.AreEqual(expectedCollections, actualCollections);
        }
        [TestMethod]
        public async Task TestCollectionsAPISearchIntegration()
        {
            // Arrange
            string search = "c#";

            // Act
            IHttpActionResult collectionResult = controller.Get(search: search);
            var message = await collectionResult.ExecuteAsync(new System.Threading.CancellationToken());
            var collections = await message.Content.ReadAsAsync<IEnumerable<Collection>>();
            var actualCollections = new List<Collection>(collections);

            var expectedCollections = Logic.Library.Search(actualCollections, search);

            // Assert
            CollectionAssert.AreEqual(expectedCollections, actualCollections);
        }
    }
}
