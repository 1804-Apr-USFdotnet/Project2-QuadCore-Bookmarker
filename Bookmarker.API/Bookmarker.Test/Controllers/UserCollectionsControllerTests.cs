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
        public void GetCollectionByIndexTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCollectionByIdTest()
        {
            Assert.Fail();
        }
    }
}