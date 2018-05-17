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
        public void ModelsIntegrationTest()
        {
            // Arrange
            User u1 = new User("u1", "p1", "p1@mail.com");
            User u2 = new User("u2", "p2", "p2@mail.com");
            Collection c1 = new Collection("c1", "d1", u1);
            Collection c2 = new Collection("c2", "d2", u2);
            Bookmark b1 = new Bookmark("b1", c1, "b1.com");
            Bookmark b2 = new Bookmark("b2", c1, "b2.com");
            Bookmark b3 = new Bookmark("b3", c2, "b3.com");
            Bookmark b4 = new Bookmark("b4", null, "b4.com");

            // Act
            int u1NumColls = u1.Collections.Count;
            int u1ExpectedNumColls = 1;

            int c1NumBms = c1.Bookmarks.Count;
            int c1ExpectedNumBms = 2;

            // Assert
            // Unique Guids
            Assert.AreNotEqual(u1.Id, u2.Id);
            Assert.AreNotEqual(u2.Id, c1.Id);
            Assert.AreNotEqual(c1.Id, c2.Id);
            Assert.AreNotEqual(c2.Id, b1.Id);
            Assert.AreNotEqual(b1.Id, b2.Id);
            Assert.AreNotEqual(b2.Id, b3.Id);
            Assert.AreNotEqual(b3.Id, b4.Id);

            Assert.AreEqual(u1ExpectedNumColls, u1NumColls);
            Assert.AreEqual(c1ExpectedNumBms, c1NumBms);

            Assert.AreEqual(36, b4.Collection.Owner.Id.ToString().Length);
            Assert.AreEqual(1, b4.Collection.Owner.Collections.Count);
            Assert.AreEqual(1, b4.Collection.Bookmarks.Count);
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
