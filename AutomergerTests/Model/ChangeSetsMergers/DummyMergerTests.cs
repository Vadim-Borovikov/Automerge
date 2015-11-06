using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class DummyMergerTests
    {
        [TestMethod()]
        public void MergeTest()
        {
            var merger = new DummyMerger();

            var changes1 = new Dictionary<int, IMergableChange>();
            changes1.Add(1, new Addition(1, new string[] { "10" }));
            changes1.Add(2, new Removal(2, 1));
            changes1.Add(3, new Replacement(3, 2, new string[] { "20" }));

            var changes2 = new Dictionary<int, IMergableChange>();
            changes2.Add(1, new Addition(1, new string[] { "30" }));
            changes2.Add(3, new Replacement(3, 2, new string[] { "40" }));

            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(null, changes2));
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(changes1, null));

            IDictionary<int, IChange> result = merger.Merge(changes1, changes2);
            CheckDictionaries(changes1, result);

            result = merger.Merge(changes2, changes1);
            CheckDictionaries(changes2, result);
        }

        private void CheckDictionaries(Dictionary<int, IMergableChange> expected,
                                       IDictionary<int, IChange> actual)
        {
            Assert.IsTrue(actual.Keys.SequenceEqual(actual.Keys));
            foreach (int key in actual.Keys)
            {
                Assert.AreEqual(actual[key], expected[key]);
            }
        }
    }
}
