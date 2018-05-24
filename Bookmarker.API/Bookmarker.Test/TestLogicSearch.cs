using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookmarker.Models;
using Bookmarker.Logic;

namespace Bookmarker.Test
{
    [TestClass]
    public class TestLogicSearch
    {
        [TestMethod]
        public void TestSearchUserByName()
        {
            User a = new User() { Username = "bob" };
            User b = new User() { Username = "billybob" };
            User c = new User() { Username = "foobar" };

            List<User> testList = new List<User>() { a, b, c };
            List<User> expected = new List<User>() { a, b };

            string sort = "bob";

            List<User> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
        [TestMethod]
        public void TestSearchUserByEmail()
        {
            User a = new User() { Email = "foobar@yahoo.com" };
            User b = new User() { Email = "bob@gmail.com" };
            User c = new User() { Email = "billybob@gmail.com" };

            List<User> testList = new List<User>() { a, b, c };
            List<User> expected = new List<User>() { b, c };

            string sort = "gmail.com";

            List<User> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
        [TestMethod]
        public void TestSearchCollectionByName()
        {
            Collection a = new Collection() { Name = "foo" };
            Collection b = new Collection() { Name = "bar" };
            Collection c = new Collection() { Name = "foobar" };

            List<Collection> testList = new List<Collection> { a, b, c};
            List<Collection> expected = new List<Collection> { a, c };

            string sort = "foo";

            List<Collection> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
        [TestMethod]
        public void TestSearchCollectionByBookmarks()
        {
            Collection a = new Collection() { Name = "a" };
            Collection b = new Collection() { Name = "b" };
            Collection c = new Collection() { Name = "c" };

            Bookmark a1 = new Bookmark() { Name = "bog", Collection = a };
            Bookmark a2 = new Bookmark() { Name = "foo", Collection = a };
            Bookmark b1 = new Bookmark() { Name = "us", Collection = b };
            Bookmark b2 = new Bookmark() { Name = "bar", Collection = b };
            Bookmark c1 = new Bookmark() { Name = "bogus", Collection = c };
            Bookmark c2 = new Bookmark() { Name = "test", Collection = c };

            a.Bookmarks = new List<Bookmark> { a1, a2 };
            b.Bookmarks = new List<Bookmark> { b1, b2 };
            c.Bookmarks = new List<Bookmark> { c1, c2 };

            List<Collection> testList = new List<Collection> { a, b, c };
            List<Collection> expected = new List<Collection> { b, c };

            string sort = "us";

            List<Collection> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
        [TestMethod]
        public void TestSearchBookmarkByName()
        {
            Bookmark a = new Bookmark() { Name = "foo" };
            Bookmark b = new Bookmark() { Name = "bar" };
            Bookmark c = new Bookmark() { Name = "foobar" };

            List<Bookmark> testList = new List<Bookmark> { a, b, c };
            List<Bookmark> expected = new List<Bookmark> { a, c };

            string sort = "foo";

            List<Bookmark> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
        [TestMethod]
        public void TestSearchBookmarkByURL()
        {
            Bookmark a = new Bookmark() { URL = "https://google.com/bogus/" };
            Bookmark b = new Bookmark() { URL = "https://google.com/foobar/" };
            Bookmark c = new Bookmark() { URL = "https://microsoft.com/" };

            List<Bookmark> testList = new List<Bookmark> { a, b, c };
            List<Bookmark> expected = new List<Bookmark> { a, b };

            string sort = "google.com";

            List<Bookmark> actual = Library.Search(testList, sort);

            CollectionAssert.AreEquivalent(actual, expected);
        }
    }
}
