using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ReplacementTests
    {
        [TestMethod()]
        public void ReplacementTest()
        {
            string[] content = new string[1] { "Test" };

            MyAssert.Throws<ArgumentException>(() => new Replacement(-1, 1, content));

            MyAssert.Throws<ArgumentException>(() => new Replacement(1, -1, content));
            MyAssert.Throws<ArgumentException>(() => new Replacement(1, 0, content));

            MyAssert.Throws<ArgumentNullException>(() => new Replacement(1, 1, null));

            var replacement = new Replacement(0, 1, content);

            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(content.SequenceEqual(replacement.NewContent));
        }
    }
}