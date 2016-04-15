using System;
using System.Collections.Generic;
using System.Linq;
using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automerger.Model.Tests
{
    [TestClass]
    public class ChangeSetApplierTests
    {
        [TestMethod]
        public void ApplyTest()
        {
            var changes = new Dictionary<int, IChange>();
            var source = new string[0];
            string[] content = { "1", "2", "3" };

            string[] source1 = source;
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(null, source1));
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(changes, null));

            changes[0] = new Replacement(0, 1, content);
            string[] source2 = source;
            MyAssert.Throws<ArgumentOutOfRangeException>(() => ChangeSetApplier.Apply(changes, source2));

            source = new[] { "0" };
            changes[0] = new Replacement(0, 2, content);
            string[] source3 = source;
            MyAssert.Throws<ArgumentOutOfRangeException>(() => ChangeSetApplier.Apply(changes, source3));

            source = new string[0];
            changes[0] = new Addition(0, content);
            ChangeSetApplier.Apply(changes, source);

            Assert.IsTrue(changes.Remove(0));
            source = new[] { "0" };
            changes[1] = new Addition(1, content);
            string[] result = ChangeSetApplier.Apply(changes, source);
            var expected = new List<string>();
            expected.AddRange(source);
            expected.AddRange(content);
            Assert.IsTrue(expected.SequenceEqual(result));
        }
    }
}
