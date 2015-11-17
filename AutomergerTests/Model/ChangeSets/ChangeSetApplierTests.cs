using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ChangeSetApplierTests
    {
        [TestMethod()]
        public void ApplyTest()
        {
            var changes = new Dictionary<int, IChange>();
            var source = new string[0];
            string[] content = new string[] { "1", "2", "3" };

            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(null, source));
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(changes, null));

            changes[0] = new Replacement(0, 1, content);
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplier.Apply(changes, source));

            source = new string[] { "0" };
            changes[0] = new Replacement(0, 2, content);
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplier.Apply(changes, source));

            source = new string[0];
            changes[0] = new Addition(0, content);
            MyAssert.ThrowsNothing(() => ChangeSetApplier.Apply(changes, source));

            Assert.IsTrue(changes.Remove(0));
            source = new string[] { "0" };
            changes[1] = new Addition(1, content);
            string[] result = ChangeSetApplier.Apply(changes, source);
            var expected = new List<string>();
            expected.AddRange(source);
            expected.AddRange(content);
            Assert.IsTrue(expected.SequenceEqual(result));
        }
    }
}
