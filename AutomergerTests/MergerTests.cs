using System;
using System.Collections.Generic;
using System.Linq;
using Automerge;
using Automerge.Changes;
using Automerge.ChangesetsMergers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergeTests
{
    [TestClass]
    public class MergerTests
    {
        private readonly ConflictBlocks _conflictBlocks;
        private readonly CustomMerger _merger;

        public MergerTests()
        {
            _conflictBlocks = Common.LoadBlocks();
            _merger = new CustomMerger(_conflictBlocks);
        }

        /// <summary>
        /// Merge method should throw ArgumentNullException correctly
        /// </summary>
        [TestMethod]
        public void TestNull()
        {
            string[] empty = { };
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(null, empty, empty, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, null, empty, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, null, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, empty, null));
            Merger.Merge(empty, empty, empty, _merger);
        }

        // We need the changes first on 2nd file prior to 1st, and then the other way around
        [TestMethod]
        public void SwapsTest()
        {
            string[] source = { "0", "1", "2", "3", "4" };
            string[] removes1 = { "0", "2", "3" };
            string[] removes2 = { "2" };

            string[] conflict1 = GenerateConflictResult(new[] { "0", "1" }, new[] { "0" }, new string[0]).ToArray();
            string[] conflict2 = GenerateConflictResult(new[] { "3", "4" }, new[] { "3" }, new string[0]).ToArray();

            var result = new List<string>(conflict1) { "2" };
            result.AddRange(conflict2);
            TestMerge(source, removes1, removes2, result.ToArray());

            conflict1 = GenerateConflictResult(new[] { "0", "1" }, new string[0], new[] { "0" }).ToArray();
            conflict2 = GenerateConflictResult(new[] { "3", "4" }, new string[0], new[] { "3" }).ToArray();

            result = new List<string>(conflict1) { "2" };
            result.AddRange(conflict2);
            TestMerge(source, removes2, removes1, result.ToArray());
        }

        // Two identical changes shouldn't yield a conflict.
        #region identical
        [TestMethod]
        public void TestIdenticalAdds()
        {
            string[] source = { "0" };
            string[] add = { "0", "1" };
            TestMerge(source, add, add, add);
        }

        [TestMethod]
        public void TestIdenticalRemoves()
        {
            string[] source = { "0", "1" };
            string[] remove = { "0" };
            TestMerge(source, remove, remove, remove);
        }

        [TestMethod]
        public void TestIdenticalReplaces()
        {
            string[] source = { "0", "1" };
            string[] replace = { "0", "2" };
            TestMerge(source, replace, replace, replace);
        }
        #endregion

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
            TestMerge(source, add, remove, result, true);
        }

        [TestMethod]
        public void TestAddRemoveWithCollision()
        {
            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] remove = { };
            TestMergeWithConflict(source, add, remove);
        }

        [TestMethod]
        public void TestAddReplaceWithCollision()
        {
            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] replace = { "3" };
            TestMergeWithConflict(source, add, replace);
        }

        [TestMethod]
        public void TestRemovesWithCollision()
        {
            string[] source = { "0", "1" };
            string[] remove1 = { };
            string[] remove2 = { "0" };
            TestMergeWithConflict(source, remove1, remove2);
        }

        [TestMethod]
        public void TestRemoveReplaceWithCollision()
        {
            string[] source = { "0", "1" };
            string[] remove = { };
            string[] replace = { "0", "2" };
            TestMergeWithConflict(source, remove, replace);
        }

        [TestMethod]
        public void TestReplacesWithCollision()
        {
            string[] source = { "0", "1" };
            string[] replace1 = { "2" };
            string[] replace2 = { "3" };
            TestMergeWithConflict(source, replace1, replace2);
        }
        #endregion

        // Two changes without collision shouldn't yield a conflict.
        // exception: two different additions before same line yield a confilct
        #region without collision
        [TestMethod]
        public void TestAddsOnSame()
        {
            string[] source = { "0" };
            string[] add1 = { "1", "0" };
            string[] add2 = { "2", "0" };

            IEnumerable<string> conflict = GenerateConflictResult(new string[0], new[] { "1" }, new[] { "2" });
            var result = new List<string>(conflict) { "0" };
            TestMerge(source, add1, add2, result.ToArray());

            conflict = GenerateConflictResult(new string[0], new[] { "2" }, new[] { "1" });
            result = new List<string>(conflict) { "0" };
            TestMerge(source, add2, add1, result.ToArray());
        }

        [TestMethod]
        public void TestAdds()
        {
            string[] source = { "0", "1" };
            string[] add1 = { "2", "0", "1" };
            string[] add2 = { "0", "3", "1" };
            string[] result = { "2", "0", "3", "1" };
            TestMerge(source, add1, add2, result, true);
        }

        [TestMethod]
        public void TestAddRemove()
        {
            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] remove = { "0" };
            string[] result = { "2", "0" };
            TestMerge(source, add, remove, result, true);
        }

        [TestMethod]
        public void TestAddReplace()
        {
            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] replace = { "0", "3" };
            string[] result = { "2", "0", "3" };
            TestMerge(source, add, replace, result, true);
        }

        [TestMethod]
        public void TestRemoves()
        {
            string[] source = { "0", "1" };
            string[] remove1 = { "1" };
            string[] remove2 = { "0" };
            string[] result = { };
            TestMerge(source, remove1, remove2, result, true);
        }

        [TestMethod]
        public void TestRemoveReplace()
        {
            string[] source = { "0", "1" };
            string[] remove = { "1" };
            string[] replace = { "0", "2" };
            string[] result = { "2" };
            TestMerge(source, remove, replace, result, true);
        }

        [TestMethod]
        public void TestReplaces()
        {
            string[] source = { "0", "1" };
            string[] replace1 = { "2", "1" };
            string[] replace2 = { "0", "3" };
            string[] result = { "2", "3" };
            TestMerge(source, replace1, replace2, result, true);
        }
        #endregion

        #region helpers
        /// <summary>
        /// Tests the merge.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed1">The first changed content.</param>
        /// <param name="changed2">The second changed content.</param>
        /// <param name="expectedText">The expected text.</param>
        private void TestMerge(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                               IReadOnlyList<string> changed2, string[] expectedText)
        {
            Result result = Merger.Merge(source, changed1, changed2, _merger);
            string[] actualText = result.Text.ToArray();
            Assert.IsNotNull(actualText);
            CollectionAssert.AreEqual(expectedText, actualText);
        }

        /// <summary>
        /// Tests the merge.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed1">The first changed content.</param>
        /// <param name="changed2">The second changed content.</param>
        /// <param name="expectedText">The expected text.</param>
        /// <param name="symmetric">Should reversed merge match the original one.</param>
        private void TestMerge(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                               IReadOnlyList<string> changed2, string[] expectedText, bool symmetric)
        {
            TestMerge(source, changed1, changed2, expectedText);
            if (symmetric)
            {
                TestMerge(source, changed2, changed1, expectedText);
            }
        }

        /// <summary>
        /// Tests the merge with conflict.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="changed1">The changed1.</param>
        /// <param name="changed2">The changed2.</param>
        private void TestMergeWithConflict(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                                           IReadOnlyList<string> changed2)
        {
            string[] result = GenerateConflictResult(source, changed1, changed2).ToArray();
            TestMerge(source, changed1, changed2, result);
            result = GenerateConflictResult(source, changed2, changed1).ToArray();
            TestMerge(source, changed2, changed1, result);
        }

        /// <summary>
        /// Generates the conflict result.
        /// </summary>
        /// <param name="originalBlock">The original block.</param>
        /// <param name="changedBlock1">The changed block1.</param>
        /// <param name="changedBlock2">The changed block2.</param>
        /// <returns></returns>
        private IEnumerable<string> GenerateConflictResult(IEnumerable<string> originalBlock,
                                                           IEnumerable<string> changedBlock1,
                                                           IEnumerable<string> changedBlock2)
        {
            yield return _conflictBlocks.ConflictBlockBegin;
            yield return _conflictBlocks.ConflictBlockSource;

            foreach (string line in originalBlock)
            {
                yield return line;
            }

            yield return _conflictBlocks.ConflictBlockChanged1;

            foreach (string line in changedBlock1)
            {
                yield return line;
            }

            yield return _conflictBlocks.ConflictBlockChanged2;

            foreach (string line in changedBlock2)
            {
                yield return line;
            }

            yield return _conflictBlocks.ConflictBlockEnd;
        }
        #endregion helpers
    }
}