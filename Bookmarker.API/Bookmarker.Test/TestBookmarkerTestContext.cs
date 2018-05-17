﻿using System;
using System.Collections;
using System.Collections.Generic;
using Bookmarker.Models;
using Bookmarker.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bookmarker.Test
{
    [TestClass]
    public class TestBookmarkerTestContext
    {
        private readonly BookmarkerTestContext _testContext;

        public TestBookmarkerTestContext()
        {
            _testContext = new BookmarkerTestContext();
        }

        [TestMethod]
        public void TestUserTable()
        {
            // Arrange
            Repository<User> users = new Repository<User>(_testContext);
            string expectedName = "smith";

            // Act
            IEnumerator<User> userEnum = users.Table.GetEnumerator();
            userEnum.MoveNext();
            string actualName = userEnum.Current.Username;

            // Assert
            Assert.AreEqual(expectedName, actualName);

            userEnum.Dispose();
        }

        [TestMethod]
        public void TestCollectionTable()
        {
            // Arrange
            Repository<Collection> collections = new Repository<Collection>(_testContext);
            string expectedName = "c#";

            // Act
            IEnumerator<Collection> collEnum = collections.Table.GetEnumerator();
            collEnum.MoveNext();
            string actualName = collEnum.Current.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);

            collEnum.Dispose();
        }

        [TestMethod]
        public void TestBookmarkTable()
        {
            // Arrange
            Repository<Bookmark> bookmarks = new Repository<Bookmark>(_testContext);
            string expectedName = "c# intro";

            // Act
            IEnumerator<Bookmark> bmEnum = bookmarks.Table.GetEnumerator();
            bmEnum.MoveNext();
            string actualName = bmEnum.Current.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);

            bmEnum.Dispose();
        }

        [TestMethod]
        public void TestUserGetById()
        {
            // Arrange
            Repository<User> userRepo = new Repository<User>(_testContext);
            IEnumerator<User> userEnum = userRepo.Table.GetEnumerator();
            userEnum.MoveNext();
            Guid actualGuid = userEnum.Current.Id;
            string expectedName = userEnum.Current.Username;

            // Act
            string actualName = userRepo.GetById(actualGuid).Username;

            // Assert
            Assert.AreEqual(expectedName, actualName);

            userEnum.Dispose();
        }

        [TestMethod]
        public void TestUserInsert()
        {
            // Arrange
            Repository<User> userRepo = new Repository<User>(_testContext);
            IEnumerator<User> userEnum = userRepo.Table.GetEnumerator();

            // Act
            int userCount = 0;
            while(userEnum.MoveNext())
            {
                userCount++;
            }

            // Assert
            Assert.AreEqual(2, userCount);

            //////////////////////////////////////////////////////////////////

            // Arrange
            userRepo.Insert(new User("john the third", "mypw", "john3@mail.com"));
            userEnum = userRepo.Table.GetEnumerator();

            // Act
            userCount = 0;
            while(userEnum.MoveNext())
            {
                userCount++;
            }

            // Assert
            Assert.AreEqual(3, userCount);

            userEnum.Dispose();
        }
    }
}
