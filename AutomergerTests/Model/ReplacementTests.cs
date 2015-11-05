using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ReplacementTests
    {
        [TestMethod()]
        public void ReplacementTest()
        {
            string[] content = new string[1] { "Test" };

            MyAssert.Throws<ArgumentException>(() => new Replacement(-1, content, 1));

            MyAssert.Throws<ArgumentNullException>(() => new Replacement(1, null, 1));

            MyAssert.Throws<ArgumentException>(() => new Replacement(1, content, -1));
            MyAssert.Throws<ArgumentException>(() => new Replacement(1, content, 0));

            var replacement = new Replacement(0, content, 1);

            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.Finish == 1);
            CollectionAssert.AreEqual(content, replacement.NewContent);
        }
    }
}