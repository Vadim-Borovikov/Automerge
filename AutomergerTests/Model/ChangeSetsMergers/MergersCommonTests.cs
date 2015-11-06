using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model.Tests
{
    public static class MergersCommonTests
    {
        public static void CommonTest(IChangeSetMerger merger)
        {
            var changes1 = new Dictionary<int, IMergableChange>();
            var changes2 = new Dictionary<int, IMergableChange>();
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(null, changes2, 0));
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(changes1, null, 0));

            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => merger.Merge(changes1, changes2, -1));

            changes1[1] = new Removal(1, 1);

            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => merger.Merge(changes1, changes2, 1));
            MyAssert.ThrowsNothing(() => merger.Merge(changes1, changes2, 2));
        }

        public static void CheckDictionaries<T1, T2>(IDictionary<int, T1> expected,
                                                     IDictionary<int, T2> actual)
        {
            CollectionAssert.AreEquivalent(expected.Keys.ToList(), actual.Keys.ToList());
            foreach (int key in expected.Keys)
            {
                Assert.AreEqual(expected[key], actual[key]);
            }
        }
    }
}