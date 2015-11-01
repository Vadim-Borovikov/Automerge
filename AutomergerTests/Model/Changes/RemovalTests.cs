using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automerger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authomerger.Tests;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class RemovalTests
    {
        [TestMethod()]
        public void RemovalTest()
        {
            MyAssert.Throws<ArgumentException>(() => new Removal(-1, 1));

            MyAssert.Throws<ArgumentException>(() => new Removal(1, -1));

            var removal = new Removal(0, 1);

            Assert.IsTrue(removal.Line == 0);
            Assert.IsTrue(removal.LinesAmount == 1);
        }
    }
}