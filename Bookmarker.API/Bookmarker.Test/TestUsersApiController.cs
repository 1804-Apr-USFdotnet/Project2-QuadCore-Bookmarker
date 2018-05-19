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
        public async Task TestUsersGetAsync()
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
    }
}
