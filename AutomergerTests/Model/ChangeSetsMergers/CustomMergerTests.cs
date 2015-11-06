using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class CustomMergerTests
    {
        [TestMethod()]
        public void CustomMergeTest()
        {
            var merger = new CustomMerger();
            MergersCommonTests.CommonTest(merger);

            // Empty source
            var source = new string[0];
            var changesAddition1 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Addition(0, new string[] { "10" }) },
            };
            var changesAddition2 = new Dictionary<int, IMergableChange>();
            MergersCommonTests.CheckBothSides(merger, source, changesAddition1, changesAddition2,
                                              changesAddition1);

            changesAddition2 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Addition(0, new string[] { "20" }) },
            };
            MergersCommonTests.CheckConflict(merger, source, changesAddition1, changesAddition2);

            // Clean merge
            source = new string[10];
            var changes1 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Addition(0, new string[] { "10" }) },
                { 3, new Removal(3, 1) },
                { 8, new Replacement(8, 1, new string[] { "30", "40" }) }
            };
            var changes2 = new Dictionary<int, IMergableChange>()
            {
                { 1, new Removal(1, 1) },
                { 5, new Replacement(5, 2, new string[] { "20" }) },
                { 9, new Addition(9, new string[] { "50" }) }
            };
            var expected = new Dictionary<int, IChange>()
            {
                { 0, new Addition(0, new string[] { "10" }) },
                { 1, new Removal(1, 1) },
                { 3, new Removal(3, 1) },
                { 5, new Replacement(5, 2, new string[] { "20" }) },
                { 8, new Replacement(8, 1, new string[] { "30", "40" }) },
                { 9, new Addition(9, new string[] { "50" }) }
            };
            MergersCommonTests.CheckBothSides(merger, source, changes1, changes2, expected);

            source = new string[] { "0", "1", "2", "3" };
            changesAddition1 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Addition(0, new string[] { "10" }) },
            };
            changesAddition2 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Addition(0, new string[] { "20", "30" }) }
            };
            var changesRemoval1 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Removal(0, 2) },
            };
            var changesRemoval2 = new Dictionary<int, IMergableChange>()
            {
                { 1, new Removal(1, 3) },
            };
            var changesReplacement1 = new Dictionary<int, IMergableChange>()
            {
                { 0, new Replacement(0, 2, new string[] { "40" }) },
            };
            var changesReplacement2 = new Dictionary<int, IMergableChange>()
            {
                { 1, new Replacement(1, 3, new string[] { "50", "60" }) }
            };

            IDictionary<int, IChange> result = merger.Merge(changesAddition1, changesAddition1,
                                                            source);
            MergersCommonTests.CheckDictionaries(changesAddition1, result);
            result = merger.Merge(changesRemoval1, changesRemoval1, source);
            MergersCommonTests.CheckDictionaries(changesRemoval1, result);
            result = merger.Merge(changesReplacement1, changesReplacement1, source);
            MergersCommonTests.CheckDictionaries(changesReplacement1, result);

            expected = new Dictionary<int, IChange>()
            {
                { 0, new Replacement(0, 2, new string[] { "10" }) },
            };
            MergersCommonTests.CheckBothSides(merger, source, changesAddition1, changesRemoval1,
                                              expected);

            MergersCommonTests.CheckConflict(merger, source, changesAddition1, changesAddition2);
            MergersCommonTests.CheckConflict(merger, source, changesAddition1,
                changesReplacement1);
            MergersCommonTests.CheckConflict(merger, source, changesRemoval1, changesRemoval2);
            MergersCommonTests.CheckConflict(merger, source, changesRemoval1, changesReplacement1);
            MergersCommonTests.CheckConflict(merger, source, changesReplacement1,
                changesReplacement2);

            source = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            changes1 = new Dictionary<int, IMergableChange>
            {
                { 0, new Replacement(0, 1, new string[] { "10" }) },
                { 2, new Addition(2, new string[] { "20" }) },
                { 3, new Removal(3, 1) },
                { 7, new Addition(7, new string[] { "70", "80" }) },
                { 8, new Removal(8, 2) }
            };
            changes2 = new Dictionary<int, IMergableChange>
            {
                { 0, new Replacement(0, 1, new string[] { "10" }) },
                { 2, new Addition(2, new string[] { "30" }) },
                { 4, new Replacement(4, 2, new string[] { "40", "50", "60" }) },
                { 7, new Removal(7, 1) },
                { 9, new Replacement(9, 2, new string[] { "90", "100" }) }
            };
            expected = new Dictionary<int, IChange>()
            {
                { 0, new Replacement(0, 1, new string[] { "10" }) },
                { 2, new Conflict(changes1[2], changes2[2], source) },
                { 3, new Removal(3, 1) },
                { 4, new Replacement(4, 2, new string[] { "40", "50", "60" }) },
                { 7, new Replacement(7, 1, new string[] { "70", "80" }) },
                { 8, new Conflict(changes1[8], changes2[9], source) },
            };
            result = merger.Merge(changes1, changes2, source);
            MergersCommonTests.CheckDictionaries(expected, result);
        }
    }
}
