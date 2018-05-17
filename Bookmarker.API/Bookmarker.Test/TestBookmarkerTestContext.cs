using System;
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
            string actualName = users.Table.GetEnumerator().Current.Username;

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }
    }
}
