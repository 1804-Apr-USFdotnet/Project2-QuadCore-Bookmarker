using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarker.Test
{
    [TestClass]
    public class FakeTestToBreakJenkins
    {
        [TestMethod]
        public void BreakJenkins()
        {
            Assert.AreEqual(1, 0);
        }
    }
}
