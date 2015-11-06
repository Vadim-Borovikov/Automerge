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

            var changes1 = new Dictionary<int, IMergableChange>()
            {
                { 1, new Addition(1, new string[] { "10" }) },
                { 2, new Removal(2, 1) },
                { 3, new Replacement(3, 2, new string[] { "20" }) }
            };

            var changes2 = new Dictionary<int, IMergableChange>()
            {
                { 1, new Addition(1, new string[] { "30" }) },
                { 3, new Replacement(3, 2, new string[] { "40" }) }
            };

            var source = new string[5];
            IDictionary<int, IChange> result = merger.Merge(changes1, changes2, source);
            MergersCommonTests.CheckDictionaries(changes1, result);

            result = merger.Merge(changes2, changes1, source);
            MergersCommonTests.CheckDictionaries(changes2, result);
        }
    }
}
