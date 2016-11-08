﻿using System;
using Automerger.Changes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergerTests.Changes
{
    [TestClass]
    public class RemovalTests
    {
        [TestMethod]
        public void RemovalTest()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Removal(-1, 1));

            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Removal(1, -1));

            var removal = new Removal(0, 1);

            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.AfterFinish == 1);
        }
    }
}