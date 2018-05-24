using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookmarker.Models;
using Bookmarker.Logic;

namespace Bookmarker.Test
{
    [TestClass]
    public class TestLogicSort
    {
        private static readonly User u1 = new User() { Username = "aaa", Created = new DateTime(1, 1, 1), Modified = new DateTime(3, 3, 3) };
        private static readonly User u2 = new User() { Username = "aa", Created = new DateTime(2, 2, 2), Modified = new DateTime(2, 2, 2) };
        private static readonly User u3 = new User() { Username = "bbb", Created = new DateTime(1, 1, 2), Modified = new DateTime(5, 5, 5) };
        private static readonly User u4 = new User() { Username = "ccc", Created = new DateTime(3, 3, 3), Modified = new DateTime(1, 1, 1) };
        private static readonly User u5 = new User() { Username = "ddd", Created = new DateTime(4, 4, 4), Modified = new DateTime(4, 4, 4) };
        
        private static readonly Collection c1 = new Collection() { Name = "bbb", Owner = u1, Private = true };
        private static readonly Collection c2 = new Collection() { Name = "aaa", Owner = u2, Private = true };
        private static readonly Collection c3 = new Collection() { Name = "ccc", Owner = u3, Private = true };
        private static readonly Collection c4 = new Collection() { Name = "ddd", Owner = u4, Private = true };
        private static readonly Collection c5 = new Collection() { Name = "eee", Owner = u5, Private = true };
        private static readonly Collection c6 = new Collection() { Name = "fff", Owner = u1, Private = true };
        
        private static readonly Bookmark b1 = new Bookmark() { Name = "bbb", URL = "jjj", Collection = c1 };
        private static readonly Bookmark b2 = new Bookmark() { Name = "aaa", URL = "iii", Collection = c2 };
        private static readonly Bookmark b3 = new Bookmark() { Name = "ccc", URL = "hhh", Collection = c3 };
        private static readonly Bookmark b4 = new Bookmark() { Name = "ddd", URL = "ggg", Collection = c4 };
        private static readonly Bookmark b5 = new Bookmark() { Name = "eee", URL = "fff", Collection = c5 };
        private static readonly Bookmark b6 = new Bookmark() { Name = "fff", URL = "eee", Collection = c6 };
        private static readonly Bookmark b7 = new Bookmark() { Name = "ggg", URL = "ddd", Collection = c1 };
        private static readonly Bookmark b8 = new Bookmark() { Name = "hhh", URL = "ccc", Collection = c2 };
        private static readonly Bookmark b9 = new Bookmark() { Name = "iii", URL = "aaa", Collection = c2 };
        private static readonly Bookmark b0 = new Bookmark() { Name = "jjj", URL = "bbb", Collection = c4 };
        
        private readonly List<User> testUsers;
        private readonly List<Collection> testCollections;
        private readonly List<Bookmark> testBookmarks;

        public TestLogicSort()
        {
            testUsers = new List<User>() { u1, u2, u3, u4, u5 };
            testCollections = new List<Collection>() { c1, c2, c3, c4, c5, c6 };
            testBookmarks = new List<Bookmark>() { b1, b2, b3, b4, b5, b6, b7, b8, b9, b0 };

            u1.Collections = new List<Collection>() { c1, c6 };
            u2.Collections = new List<Collection>() { c2 };
            u3.Collections = new List<Collection>() { c3 };
            u4.Collections = new List<Collection>() { c4 };
            u5.Collections = new List<Collection>() { c5 };

            c1.Bookmarks = new List<Bookmark>() { b1, b7 };
            c2.Bookmarks = new List<Bookmark>() { b2, b8, b9 };
            c3.Bookmarks = new List<Bookmark>() { b3 };
            c4.Bookmarks = new List<Bookmark>() { b4, b0 };
            c5.Bookmarks = new List<Bookmark>() { b5 };
            c6.Bookmarks = new List<Bookmark>() { b6 };

            c1.ThumbUp(); c1.ThumbUp(); c1.ThumbUp();
            c2.ThumbUp();
            c3.ThumbUp(); c3.ThumbUp();
            c4.ThumbUp(); c4.ThumbUp();
            c5.ThumbUp(); c5.ThumbUp(); c5.ThumbUp(); c5.ThumbUp();
            c6.ThumbUp();

            b1.ThumbUp(); b1.ThumbUp(); b1.ThumbUp();
            b2.ThumbUp();
            b3.ThumbUp();
            b4.ThumbUp(); b4.ThumbUp(); b4.ThumbUp(); b4.ThumbUp();
            b5.ThumbUp();
            b6.ThumbUp();
            b7.ThumbUp(); b7.ThumbUp(); b7.ThumbUp();
            b8.ThumbUp();
            b9.ThumbUp(); b9.ThumbUp();
            b0.ThumbUp();
        }

        [TestMethod]
        public void TestSortUser()
        {
            string sort = "name";
            List<User> actual = new List<User>(testUsers);
            List<User> expected = new List<User> { u2, u1, u3, u4, u5};

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortCollection()
        {
            string sort = "rating";
            List<Collection> actual = new List<Collection>(testCollections);
            List<Collection> expected = new List<Collection>() { c5, c1, c3, c4, c2, c6};

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortBookmark()
        {
            string sort = "name";
            List<Bookmark> actual = new List<Bookmark>(testBookmarks);
            List<Bookmark> expected = new List<Bookmark>() { b2, b1, b3, b4, b5, b6, b7, b8, b9, b0 };

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortAsc()
        {
            string sort = "name:asc";
            List<Bookmark> actual = new List<Bookmark>(testBookmarks);
            List<Bookmark> expected = new List<Bookmark>() { b2, b1, b3, b4, b5, b6, b7, b8, b9, b0 };

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortDesc()
        {
            string sort = "owner:desc";
            List<Collection> actual = new List<Collection>(testCollections);
            List<Collection> expected = new List<Collection>() { c5, c4, c3, c1, c6, c2 };

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortMultipleTerms()
        {
            string sort = "rating,name:desc";
            List<Bookmark> actual = new List<Bookmark>(testBookmarks);
            List<Bookmark> expected = new List<Bookmark>() { b4, b7, b1, b9, b0, b8, b6, b5, b3, b2 };

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortInvalidKey()
        {
            string sort = "Bogus";
            List<Bookmark> actual = new List<Bookmark>(testBookmarks);
            List<Bookmark> expected = new List<Bookmark>(testBookmarks);

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestSortPartialValidKey()
        {
            string sort = "bogus,name,bogus";
            List<User> actual = new List<User>(testUsers);
            List<User> expected = new List<User> { u2, u1, u3, u4, u5 };

            Library.Sort(ref actual, sort);

            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
