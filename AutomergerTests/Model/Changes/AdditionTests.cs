using System;
using System.Linq;
using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automerger.Model.Tests
{
    [TestClass]
    public class AdditionTests
    {
        [TestMethod]
        public void AdditionTest()
        {
            string[] content = { "Test" };

            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Addition(-1, content));

            MyAssert.Throws<ArgumentNullException>(() => new Addition(0, null));
            MyAssert.Throws<ArgumentException>(() => new Addition(0, new string[0]));

            var addition = new Addition(1, content);

            Assert.IsTrue(addition.Start == 1);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.AfterFinish == 1);
            Assert.IsTrue(content.SequenceEqual(addition.NewContent));
        }
    }
}