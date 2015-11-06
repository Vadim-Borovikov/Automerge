using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class DummyMergerTests
    {
        [TestMethod()]
        public void DummyMergeTest()
        {
            var merger = new DummyMerger();
            MergersCommonTests.CommonTest(merger);

            var changes1 = new Dictionary<int, IMergableChange>();
            changes1[1] = new Addition(1, new string[] { "10" });
            changes1[2] = new Removal(2, 1);
            changes1[3] = new Replacement(3, 2, new string[] { "20" });

            var changes2 = new Dictionary<int, IMergableChange>();
            changes2[1] = new Addition(1, new string[] { "30" });
            changes2[3] = new Replacement(3, 2, new string[] { "40" });

            IDictionary<int, IChange> result = merger.Merge(changes1, changes2, 5);
            MergersCommonTests.CheckDictionaries(changes1, result);

            result = merger.Merge(changes2, changes1, 5);
            MergersCommonTests.CheckDictionaries(changes2, result);
        }
    }
}
