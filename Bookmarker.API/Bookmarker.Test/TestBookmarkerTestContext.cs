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
        private BookmarkerTestContext _testContext;

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
        }
    }
}
