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
    public class ReplacementTests
    {
        [TestMethod()]
        public void ReplacementTest()
        {
            string[] content = new string[1] { "Test" };

            MyAssert.Throws<ArgumentException>(() => new Replacement(-1, content, 1));
            MyAssert.Throws<ArgumentException>(() => new Replacement(0, content, 1));

            MyAssert.Throws<ArgumentNullException>(() => new Replacement(1, null, 1));

            MyAssert.Throws<ArgumentException>(() => new Replacement(1, content, -1));
            MyAssert.Throws<ArgumentException>(() => new Replacement(1, content, 0));

            var replacement = new Replacement(1, content, 1);

            Assert.IsTrue(replacement.Line == 1);
            Assert.IsTrue(replacement.LinesAmount == 1);
            CollectionAssert.AreEqual(content, replacement.NewContent);
        }
    }
}