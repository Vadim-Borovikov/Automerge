using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class AdditionTests
    {
        [TestMethod()]
        public void AdditionTest()
        {
            string[] content = new string[1] { "Test" };

            MyAssert.Throws<ArgumentException>(() => new Addition(-1, content));

            MyAssert.Throws<ArgumentNullException>(() => new Addition(0, null));

            var addition = new Addition(1, content);

            Assert.IsTrue(addition.Start == 1);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(content.SequenceEqual(addition.NewContent));
        }
    }
}