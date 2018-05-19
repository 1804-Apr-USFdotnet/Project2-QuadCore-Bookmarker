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
        private readonly UsersController controller 
            = new UsersController(new BookmarkerTestContext());

        //private static readonly HttpClient httpClient = new HttpClient();
        //private static readonly string apiPath = "http://localhost:55287/api";

        [TestMethod]
        public async Task TestUsersGetAsync()
        {
            // Arrange
            //HttpResponseMessage response = 
            //    new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

            //try
            //{
            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            //    response = await httpClient.GetAsync($"{apiPath}/Users");
            //}
            //catch
            //{
            //    Assert.Fail("API test server is not running or has incorrect path.");
            //}
            //int expectedCount = 6;
            int expectedCount = 2;
            int actualCount = 0;

            // Act
            //if (!response.IsSuccessStatusCode)
            //{
            //    Assert.Fail($"Get request failed: {response.StatusCode}");
            //}
            //var users = await response.Content.ReadAsAsync<IEnumerable<User>>();
            controller.ControllerContext.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
            IHttpActionResult userResult = controller.Get();
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualCount++;
            }
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
