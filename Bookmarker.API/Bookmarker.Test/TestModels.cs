using System;
using System.Collections.Generic;
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
            User user = new User();
            user.Username = "name"; user.Password = "pw"; user.Email = "a@b.com"; user.Created = System.DateTime.UtcNow;
            Collection coll = new Collection();
            coll.Name = "Food"; coll.Description = "Lots of Food";
            coll.Created = System.DateTime.UtcNow;
            Bookmark bm = new Bookmark();
            bm.Name = "abc"; bm.URL = "abc.com";
            bm.Created = System.DateTime.UtcNow;

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
            User u1 = new User();
            u1.Username = "u1"; u1.Password = "p1"; u1.Email = "p1@mail.com";
            u1.Collections = new List<Collection>();
            u1.Id = Guid.NewGuid();
            User u2 = new User();
            u2.Username = "u2"; u2.Password = "p2"; u2.Email = "p2@mail.com";
            u2.Collections = new List<Collection>();
            u2.Id = Guid.NewGuid();
            Collection c1 = new Collection();
            c1.Name = "c1"; c1.Description = "d1"; c1.Owner = u1;
            c1.Bookmarks = new List<Bookmark>();
            c1.Id = Guid.NewGuid();
            Collection c2 = new Collection();
            c2.Name = "c2"; c2.Description = "d2"; c2.Owner = u2;
            c2.Bookmarks = new List<Bookmark>();
            c2.Id = Guid.NewGuid();
            Bookmark b1 = new Bookmark();
            b1.Name = "b1"; b1.URL = "b1.com"; b1.Collection = c1;
            b1.Id = Guid.NewGuid();
            Bookmark b2 = new Bookmark();
            b2.Name = "b2"; b2.URL = "b2.com"; b2.Collection = c1;
            b2.Id = Guid.NewGuid();
            Bookmark b3 = new Bookmark();
            b3.Name = "b3"; b3.URL = "b3.com"; b3.Collection = c2;
            b3.Id = Guid.NewGuid();
            Bookmark b4 = new Bookmark();
            b4.Name = "b4"; b4.URL = "b4.com"; b4.Collection = c2;
            b4.Id = Guid.NewGuid();

            u1.Collections.Add(c1);
            u2.Collections.Add(c2);
            c1.Bookmarks.Add(b1);
            c1.Bookmarks.Add(b2);
            c2.Bookmarks.Add(b3);
            c2.Bookmarks.Add(b4);

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
            Assert.AreEqual(2, b4.Collection.Bookmarks.Count);
        }

        [TestMethod]
        public void TestIRatable()
        {
            // Arrange
            Collection c1 = new Collection();
            c1.Name = "c1"; c1.Description = "d1";
            Collection c2 = new Collection();
            c2.Name = "c2"; c2.Description = "d2";
            Bookmark b1 = new Bookmark();
            b1.Name = "b1"; b1.URL = "b1.com";

            // Act
            c2.ThumbUp();
            b1.ThumbUp();
            b1.ThumbUp();

            // Assert
            Assert.AreEqual(0, c1.Rating);
            Assert.AreEqual(1, c2.Rating);
            Assert.AreEqual(2, b1.Rating);
        }
    }
}
