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
        public async Task TestUsersApiGetAsync()
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

            // Assert
            Assert.AreEqual(expectedUserCount, actualUserCount);
        }

        [TestMethod]
        public async Task TestUsersApiGetByIdAsync()
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

        [TestMethod]
        public async Task TestUsersApiPostAsync()
        {
            // Arrange
            int actualUserCount = 0;

            // Act
            IHttpActionResult userResult = controller.Get();
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }

            //////////////////////////////////////////////////////////////////

            // Post -- invalid model -- expect bad request message
            User newUser = new User();
            newUser.Username = "jon";
            //newUser.Password = "badPw";
            newUser.Email = "jon@mail.com";
            newUser.Id = new Guid("55555555-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("k1", "password is too short");
            IHttpActionResult result = controller.Post(new UserAPI(newUser));
            var badPostMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badPostMessage.StatusCode);

            //newUser.Password = "okayPassword";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Post(new UserAPI(newUser));
            var goodPostMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodPostMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Arrange
            int expectedUserCount = actualUserCount + 1;
            // reset count to test again after the POST
            actualUserCount = 0;

            // Act
            userResult = controller.Get();
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }

            // Assert
            Assert.AreEqual(expectedUserCount, actualUserCount);
        }

        [TestMethod]
        public async Task TestUsersApiPutAsync()
        {
            // Find out how many users there are

            // Arrange
            int actualUserCount = 0;

            // Act
            IHttpActionResult userResult = controller.Get();
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }

            //////////////////////////////////////////////////////////////////

            // PUT a new user with invalid model - expect bad request message
            User newUser = new User();
            newUser.Username = "jon";
            //newUser.Password = "badPw";
            newUser.Email = "jon@mail.com";
            newUser.Id = new Guid("55555555-4444-4444-4444-222222222222");

            controller.ModelState.AddModelError("k1", "password is too short");
            IHttpActionResult result = controller.Put(new UserAPI(newUser));
            var badMessage = await result.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.BadRequest, badMessage.StatusCode);

            // Make new user's model valid and PUT
            //newUser.Password = "okayPassword";
            controller.ModelState.Remove("k1");
            IHttpActionResult goodResult = controller.Put(new UserAPI(newUser));
            var goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            /////////////////////////////////////////////////////////////////

            // Expect user count to be one more than before

            // Arrange
            int expectedUserCount = actualUserCount + 1;
            // reset count to test again after the PUT
            actualUserCount = 0;

            // Act
            userResult = controller.Get();
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }

            // Assert
            Assert.AreEqual(expectedUserCount, actualUserCount);

            /////////////////////////////////////////////////////////////////

            // Now do an update to existing user with PUT
            // user count shouldn't change

            // Arrange
            expectedUserCount = actualUserCount;
            // reset count to test again after the PUT
            actualUserCount = 0;

            User oldUser = new User();
            oldUser.Username = "jon";
            //oldUser.Password = "newPassword";
            oldUser.Email = "jonsNewEmail@mail.com";
            oldUser.Id = new Guid("55555555-4444-4444-4444-222222222222");

            goodResult = controller.Put(new UserAPI(oldUser));
            goodMessage = await goodResult.ExecuteAsync(new System.Threading.CancellationToken());
            Assert.AreEqual(HttpStatusCode.OK, goodMessage.StatusCode);

            // Act
            userResult = controller.Get();
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var user in users)
            {
                actualUserCount++;
            }

            // Assert
            Assert.AreEqual(expectedUserCount, actualUserCount);
        }

        [TestMethod]
        public async Task TestUsersApiDeleteAsync()
        {
            // Find out how many users there are

            // Arrange
            int actualUserCount = 0;

            // Act
            IHttpActionResult userResult = controller.Get();
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var u in users)
            {
                actualUserCount++;
            }
            
            ///////////////////////////////////////////////////////////////

            // Arrange
            Guid franksGuid = new Guid("11111111-4444-4444-4444-222222222222");
            Guid wrongGuid = new Guid("99999999-1111-4444-4444-222222222222");

            // Act
            userResult = controller.Delete(wrongGuid);
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, message.StatusCode);

            // Act
            userResult = controller.Delete(franksGuid);
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);

            ///////////////////////////////////////////////////////////////

            // Check that there is one less user

            // Arrange
            int expectedUserCount = actualUserCount - 1;
            actualUserCount = 0;

            // Act
            userResult = controller.Get();
            message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            foreach (var u in users)
            {
                actualUserCount++;
            }

            Assert.AreEqual(expectedUserCount, actualUserCount);
        }

        [TestMethod]
        public async Task TestUsersAPISortIntegration()
        {
            // Arrange
            string sort = "name:desc";

            // Act
            IHttpActionResult userResult = controller.Get(sort: sort);
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var users = await message.Content.ReadAsAsync<IEnumerable<User>>();
            var actualUsers = new List<User>(users);
            
            var expectedUsers = new List<User>(actualUsers);
            Logic.Library.Sort(ref expectedUsers, sort);
            
            // Assert
            CollectionAssert.AreEqual(expectedUsers, actualUsers);
        }
        [TestMethod]
        public async Task TestUsersApiSearchIntegration()
        {
            // Arrange
            string search = "smith";

            // Act
            IHttpActionResult userResult = controller.Get(search: search);
            var message = await userResult.ExecuteAsync(new System.Threading.CancellationToken());
            var bookmarks = await message.Content.ReadAsAsync<IEnumerable<User>>();
            var actualUsers = new List<User>(bookmarks);

            var expectedUsers = Logic.Library.Search(actualUsers, search);

            // Assert
            CollectionAssert.AreEqual(expectedUsers, actualUsers);
        }
    }
}
