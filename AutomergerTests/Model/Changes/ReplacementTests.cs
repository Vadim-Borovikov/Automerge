using System;
using System.Linq;
using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automerger.Model.Tests
{
    [TestClass]
    public class ReplacementTests
    {
        [TestMethod]
        public void ReplacementTest()
        {
            string[] content = { "Test" };

            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Replacement(-1, 1, content));

            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Replacement(0, -1, content));
            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Replacement(0, 0, content));

            MyAssert.Throws<ArgumentNullException>(() => new Replacement(0, 1, null));
            MyAssert.Throws<ArgumentException>(() => new Replacement(0, 1, new string[0]));

            var replacement = new Replacement(0, 1, content);

            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.AfterFinish == 1);
            Assert.IsTrue(content.SequenceEqual(replacement.NewContent));
        }
    }
}