using System;
using Bookmarker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bookmarker.Test
{
    [TestClass]
    public class TestModels
    {
        [TestMethod]
        public void TestABaseEntity()
        {
            // Arrange
            User user = new User("name", "pw", "a@b.com");
            Collection coll = new Collection("Food", "Lots of Food", null);
            Bookmark bm = new Bookmark("abc", null, "abc.com");

            // Act
            int userGuidLength = user.Id.ToString().Length;
            int collGuidLength = coll.Id.ToString().Length;
            int bmGuidLength = bm.Id.ToString().Length;

            // Assert
            Assert.AreEqual(DateTime.UtcNow.Minute, user.Created.Minute);
            Assert.AreEqual(null, user.Modified);
            Assert.AreEqual(36, userGuidLength);

            Assert.AreEqual(DateTime.UtcNow.Minute, coll.Created.Minute);
            Assert.AreEqual(null, coll.Modified);
            Assert.AreEqual(36, collGuidLength);

            Assert.AreEqual(DateTime.UtcNow.Minute, bm.Created.Minute);
            Assert.AreEqual(null, bm.Modified);
            Assert.AreEqual(36, bmGuidLength);
        }

        [TestMethod]
        public void TestBookmark()
        {
            Assert.AreEqual(0, 1);
        }

        [TestMethod]
        public void TestCollection()
        {
            Assert.AreEqual(0, 1);
        }

        [TestMethod]
        public void TestUser()
        {
            Assert.AreEqual(0, 1);
        }

        [TestMethod]
        public void ModelsIntegrationTest()
        {
            Assert.AreEqual(0, 1);
        }

        [TestMethod]
        public void TestIRatable()
        {
            // Arrange
            Collection c = new Collection("Recipes", "My favorite recipes", null);
            Collection c2 = new Collection("Recipes", "My favorite recipes", null);
            Bookmark bm = new Bookmark("x", null, "x.com");

            // Act
            c2.ThumbUp();
            bm.ThumbUp();
            bm.ThumbUp();

            // Assert
            Assert.AreEqual(0, c.Rating);
            Assert.AreEqual(1, c2.Rating);
            Assert.AreEqual(2, bm.Rating);
        }
    }
}
