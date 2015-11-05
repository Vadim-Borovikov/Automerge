using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
        }
    }
}