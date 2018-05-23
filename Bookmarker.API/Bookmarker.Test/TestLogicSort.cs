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
        private static readonly string n = "name", p = "password";
        
        [TestMethod]
        public void TestSortUserNameAsc()
        {
            string sort = "name:asc";

            User a = new User() { Username = "abc" };
            User b = new User() { Username = "abcd" };
            User c = new User() { Username = "mmm" };
            User d = new User() { Username = "zza" };
            User e = new User() { Username = "zzz" };

            List<User> expected = new List<User>()
            { a, b, c, d, e};
            List<User> users = new List<User>()
            { c, b, e, a, d};


            Library.Sort(ref users, sort);


            Assert.AreEqual(users, expected);
        }
        [TestMethod]
        public void TestSortUserNameDesc()
        {
            string sort = "name:desc";

            User a = new User() { Username = "abc" };
            User b = new User() { Username = "abcd" };
            User c = new User() { Username = "mmm" };
            User d = new User() { Username = "zza" };
            User e = new User() { Username = "zzz" };

            List<User> expected = new List<User>()
            { e, d, c, b, a};
            List<User> users = new List<User>()
            { c, b, e, a, d};


            Library.Sort(ref users, sort);


            Assert.AreEqual(users, expected);
        }
        [TestMethod]
        public void TestSortUserCreatedAsc()
        {
            string sort = "created:asc";

            User a = new User() { Created = new DateTime(1960, 6, 25) };
            User b = new User() { Created = new DateTime(1998, 5, 25) };
            User c = new User() { Created = new DateTime(1998, 6, 25) };
            User d = new User() { Created = new DateTime(1998, 6, 26) };
            User e = new User() { Created = new DateTime(2018, 6, 25) };

            List<User> expected = new List<User>()
            { a, b, c, d, e};
            List<User> users = new List<User>()
            { d, a, e, b, c};


            Library.Sort(ref users, sort);


            Assert.AreEqual(users, expected);
        }
        [TestMethod]
        public void TestSortUserCreatedDesc()
        {
            string sort = "created:desc";

            User a = new User() { Created = new DateTime(1960, 6, 25) };
            User b = new User() { Created = new DateTime(1998, 5, 25) };
            User c = new User() { Created = new DateTime(1998, 6, 25) };
            User d = new User() { Created = new DateTime(1998, 6, 26) };
            User e = new User() { Created = new DateTime(2018, 6, 25) };

            List<User> expected = new List<User>()
            { e, d, c, b, a};
            List<User> users = new List<User>()
            { d, a, e, b, c};


            Library.Sort(ref users, sort);


            Assert.AreEqual(users, expected);
        }
        [TestMethod]
        public void TestSortUserEditedAsc()
        {
            string sort = "Modified:asc";

            User a = new User() { Modified = new DateTime(2000, 1, 1) };
        }
    }
}
