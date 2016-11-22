using System.Collections.Generic;
using System.Linq;
using Automerge.ChangesetsMergers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergeTests.ChangesetsMergers
{
    [TestClass]
    public class CustomMergerTests
    {
        private static readonly CustomMerger Merger = new CustomMerger(Common.ConflictBlocks);

        /// <summary>
        /// Merge method should throw ArgumentNullException correctly
        /// </summary>
        [TestMethod]
        public void TestNull() => Common.TestNull(Merger);

        // We need the changes first on 2nd file prior to 1st, and then the other way around
        [TestMethod]
        public void TestSwaps()
        {
            string[] source = { "0", "1", "2", "3", "4" };
            string[] removes1 = { "0", "2", "3" };
            string[] removes2 = { "2" };

            string[] conflict1 =
                Common.GenerateConflictResult(new[] { "0", "1" }, new[] { "0" }, new string[0]).ToArray();
            string[] conflict2 =
                Common.GenerateConflictResult(new[] { "3", "4" }, new[] { "3" }, new string[0]).ToArray();

            var result = new List<string>(conflict1) { "2" };
            result.AddRange(conflict2);
            Common.TestMerge(source, removes1, removes2, result.ToArray(), Merger);

            conflict1 =
                Common.GenerateConflictResult(new[] { "0", "1" }, new string[0], new[] { "0" }).ToArray();
            conflict2 =
                Common.GenerateConflictResult(new[] { "3", "4" }, new string[0], new[] { "3" }).ToArray();

            result = new List<string>(conflict1) { "2" };
            result.AddRange(conflict2);
            Common.TestMerge(source, removes2, removes1, result.ToArray(), Merger);
        }


        // What if one change collides with several others?
        [TestMethod]
        public void TestComplexCollision()
        {
            string[] source = { "0", "1", "2", "3", "4" };
            string[] removes1 = { "2" };
            string[] removes2 = { "0", "4" };
            Common.TestMergeWithConflict(source, removes1, removes2, Merger);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Two identical changes shouldn't yield a conflict.
        #region identical
        [TestMethod]
        public void TestIdenticalAdds()
        {
            string[] source = { "0" };
            string[] add = { "0", "1" };
            Common.TestMerge(source, add, add, add, Merger);
        }

        [TestMethod]
        public void TestIdenticalRemoves()
        {
            string[] source = { "0", "1" };
            string[] remove = { "0" };
            Common.TestMerge(source, remove, remove, remove, Merger);
        }

        [TestMethod]
        public void TestIdenticalReplaces()
        {
            string[] source = { "0", "1" };
            string[] replace = { "0", "2" };
            Common.TestMerge(source, replace, replace, replace, Merger);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Two changes with collision should yield a conflict
        // exception: an addition before line from which a removal starts yields a replacement
        #region with collision
        [TestMethod]
        public void TestAddRemoveOnSame()
        {
            string[] source = { "0" };
            string[] add = { "1", "0" };
            string[] remove = { };
            string[] result = { "1" };
            Common.TestMerge(source, add, remove, result, Merger, true);
        }

        [TestMethod]
        public void TestAddRemoveWithCollision()
        {
            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] remove = { };
            Common.TestMergeWithConflict(source, add, remove, Merger);
        }

        [TestMethod]
        public void TestAddReplaceWithCollision()
        {
            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] replace = { "3" };
            Common.TestMergeWithConflict(source, add, replace, Merger);
        }

        [TestMethod]
        public void TestRemovesWithCollision()
        {
            string[] source = { "0", "1" };
            string[] remove1 = { };
            string[] remove2 = { "0" };
            Common.TestMergeWithConflict(source, remove1, remove2, Merger);
        }

        [TestMethod]
        public void TestRemoveReplaceWithCollision()
        {
            string[] source = { "0", "1" };
            string[] remove = { };
            string[] replace = { "0", "2" };
            Common.TestMergeWithConflict(source, remove, replace, Merger);
        }

        [TestMethod]
        public void TestReplacesWithCollision()
        {
            string[] source = { "0", "1" };
            string[] replace1 = { "2" };
            string[] replace2 = { "3" };
            Common.TestMergeWithConflict(source, replace1, replace2, Merger);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Two changes without collision shouldn't yield a conflict.
        // exception: two different additions before same line yield a confilct
        #region without collision
        [TestMethod]
        public void TestAddsOnSame()
        {
            string[] source = { "0" };
            string[] add1 = { "1", "0" };
            string[] add2 = { "2", "0" };

            IEnumerable<string> conflict = Common.GenerateConflictResult(new string[0], new[] { "1" }, new[] { "2" });
            var result = new List<string>(conflict) { "0" };
            Common.TestMerge(source, add1, add2, result.ToArray(), Merger);

            conflict = Common.GenerateConflictResult(new string[0], new[] { "2" }, new[] { "1" });
            result = new List<string>(conflict) { "0" };
            Common.TestMerge(source, add2, add1, result.ToArray(), Merger);
        }

        [TestMethod]
        public void TestAdds()
        {
            string[] source = { "0", "1" };
            string[] add1 = { "2", "0", "1" };
            string[] add2 = { "0", "3", "1" };
            string[] result = { "2", "0", "3", "1" };
            Common.TestMerge(source, add1, add2, result, Merger, true);
        }

        [TestMethod]
        public void TestAddRemove()
        {
            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] remove = { "0" };
            string[] result = { "2", "0" };
            Common.TestMerge(source, add, remove, result, Merger, true);
        }

        [TestMethod]
        public void TestAddReplace()
        {
            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] replace = { "0", "3" };
            string[] result = { "2", "0", "3" };
            Common.TestMerge(source, add, replace, result, Merger, true);
        }

        [TestMethod]
        public void TestRemoves()
        {
            string[] source = { "0", "1" };
            string[] remove1 = { "1" };
            string[] remove2 = { "0" };
            string[] result = { };
            Common.TestMerge(source, remove1, remove2, result, Merger, true);
        }

        [TestMethod]
        public void TestRemoveReplace()
        {
            string[] source = { "0", "1" };
            string[] remove = { "1" };
            string[] replace = { "0", "2" };
            string[] result = { "2" };
            Common.TestMerge(source, remove, replace, result, Merger, true);
        }

        [TestMethod]
        public void TestReplaces()
        {
            string[] source = { "0", "1" };
            string[] replace1 = { "2", "1" };
            string[] replace2 = { "0", "3" };
            string[] result = { "2", "3" };
            Common.TestMerge(source, replace1, replace2, result, Merger, true);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
