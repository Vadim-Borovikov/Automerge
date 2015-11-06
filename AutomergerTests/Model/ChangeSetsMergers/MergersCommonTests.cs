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
            var source = new string[0];
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(null, changes2, source));
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(changes1, null, source));
            MyAssert.Throws<ArgumentNullException>(() => merger.Merge(changes1, changes2, null));

            changes1[1] = new Removal(1, 1);

            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => merger.Merge(changes1, changes2, new string[1]));
            MyAssert.ThrowsNothing(() => merger.Merge(changes1, changes2, new string[2]));
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

        public static void CheckBothSides<T>(IChangeSetMerger merger, string[] source,
                                             IReadOnlyDictionary<int, IMergableChange> changes1,
                                             IReadOnlyDictionary<int, IMergableChange> changes2,
                                             IDictionary<int, T> expected)
        {
            IDictionary<int, IChange> result = merger.Merge(changes1, changes2, source);
            CheckDictionaries(expected, result);

            result = merger.Merge(changes2, changes1, source);
            CheckDictionaries(expected, result);
        }

        public static void CheckConflict(IChangeSetMerger merger, string[] source,
                                         IReadOnlyDictionary<int, IMergableChange> changes1,
                                         IReadOnlyDictionary<int, IMergableChange> changes2)
        {
            int key1 = changes1.Keys.First();
            int key2 = changes2.Keys.First();
            int key = Math.Min(key1, key2);
            var expected = new Dictionary<int, IChange>()
            {
                { key, new Conflict(changes1[key1], changes2[key2], source) }
            };
            IDictionary<int, IChange> result = merger.Merge(changes1, changes2, source);
            CheckDictionaries(expected, result);
        }
    }
}
