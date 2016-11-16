using Automerge.Changes;
using Automerge.ChangesetsMergers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergeTests.ChangesetsMergers
{
    [TestClass]
    public class CustomMergerTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            ConflictBlocks conflictBlocks = Common.LoadBlocks();
            var customMerger = new CustomMerger(conflictBlocks);
        }
    }
}
