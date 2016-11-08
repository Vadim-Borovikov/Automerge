using System.Collections.Generic;
using Automerger.Changes;
using Automerger.ChangeSetsMergers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergerTests.ChangeSetsMergers
{
    [TestClass]
    public class DummyMergerTests
    {
        [TestMethod]
        public void DummyMergeTest()
        {
            var merger = new DummyMerger();
            MergersCommonTests.CommonTest(merger);

            var changes1 = new Dictionary<int, IMergableChange>
            {
                { 1, new Addition(1, new[] { "10" }) },
                { 2, new Removal(2, 1) },
                { 3, new Replacement(3, 2, new[] { "20" }) }
            };

            var changes2 = new Dictionary<int, IMergableChange>
            {
                { 1, new Addition(1, new[] { "30" }) },
                { 3, new Replacement(3, 2, new[] { "40" }) }
            };

            var source = new string[5];
            IDictionary<int, IChange> result = merger.Merge(changes1, changes2, source);
            MergersCommonTests.CheckDictionaries(changes1, result);

            result = merger.Merge(changes2, changes1, source);
            MergersCommonTests.CheckDictionaries(changes2, result);
        }
    }
}
