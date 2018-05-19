using System;
using System.Collections.Generic;
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
    public class TestUsersApiController
    {
        private readonly UsersController controller;

        public TestUsersApiController()
        {
            controller = new UsersController(new BookmarkerTestContext());
            controller.ControllerContext.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task TestUsersAPIGetAsync()
        {
            // Arrange
            int expectedUserCount = 2;
            int actualUserCount = 0;

            // Act
            IHttpActionResult userResult = controller.Get();
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }
            Assert.AreEqual(expectedUserCount, actualUserCount);
        }

        [TestMethod]
        public async Task TestUsersAPIGetByIdAsync()
        {
            // Arrange
            string expectedUsername = "smith";
            Guid smithsGuid = new Guid("88888888-4444-4444-4444-222222222222");

            // Act
            IHttpActionResult userResult = controller.Get(smithsGuid);
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var user = await message.Content.ReadAsAsync<User>();

            // Assert
            Assert.AreEqual(expectedUsername, user.Username);
        }
    }
}
