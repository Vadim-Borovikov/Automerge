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
    public class AdditionTests
    {
        [TestMethod()]
        public void AdditionTest()
        {
            string[] content = new string[1] { "Test" };

            MyAssert.Throws<ArgumentException>(() => new Addition(-1, content));
            MyAssert.Throws<ArgumentException>(() => new Addition(0, content));

            MyAssert.Throws<ArgumentNullException>(() => new Addition(1, null));

            var addition = new Addition(1, content);

            Assert.IsTrue(addition.Line == 1);
            Assert.IsTrue(addition.LinesAmount == 0);
            CollectionAssert.AreEqual(content, addition.Content);
        }
    }
}