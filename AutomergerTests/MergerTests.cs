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
        /// <summary>
        /// Merge method should throw ArgumentNullException correctly
        /// </summary>
        [TestMethod]
        public void TestNull()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] empty = { };
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(null, empty, empty, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, null, empty, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, null, _merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, empty, null));
            Merger.Merge(empty, empty, empty, _merger);
        }

        // Two identical changes shouldn't yield a conflict.
        #region identical
        [TestMethod]
        public void TestIdenticalAdds()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0" };
            string[] add = { "0", "1" };
            TestMerge(source, add, add, add);
        }

        [TestMethod]
        public void TestIdenticalRemoves()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] remove = { "0" };
            TestMerge(source, remove, remove, remove);
        }

        [TestMethod]
        public void TestIdenticalReplaces()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

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
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0" };
            string[] add = { "1", "0" };
            string[] remove = { };
            string[] result = { "1" };
            TestMerge(source, add, remove, result, true);
        }

        [TestMethod]
        public void TestAddRemoveWithCollision()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] remove = { };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "0", "2", "1",
                _conflictBlocks.ConflictBlockChanged2,
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, add, remove, result);
            result = new[]
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                _conflictBlocks.ConflictBlockChanged2,
                "0", "2", "1",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, remove, add, result);
        }

        [TestMethod]
        public void TestAddReplaceWithCollision()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] add = { "0", "2", "1" };
            string[] replace = { "3" };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "0", "2", "1",
                _conflictBlocks.ConflictBlockChanged2,
                "3",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, add, replace, result);
            result = new[]
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "3",
                _conflictBlocks.ConflictBlockChanged2,
                "0", "2", "1",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, replace, add, result);
        }

        [TestMethod]
        public void TestRemovesWithCollision()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] remove1 = { };
            string[] remove2 = { "0" };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                _conflictBlocks.ConflictBlockChanged2,
                "0",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, remove1, remove2, result);
            result = new[]
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "0",
                _conflictBlocks.ConflictBlockChanged2,
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, remove2, remove1, result);
        }

        [TestMethod]
        public void TestRemoveReplaceWithCollision()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] remove = { };
            string[] replace = { "0", "2" };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                _conflictBlocks.ConflictBlockChanged2,
                "0", "2",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, remove, replace, result);
            result = new[]
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "0", "2",
                _conflictBlocks.ConflictBlockChanged2,
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, replace, remove, result);
        }

        [TestMethod]
        public void TestReplacesWithCollision()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] replace1 = { "2" };
            string[] replace2 = { "3" };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "2",
                _conflictBlocks.ConflictBlockChanged2,
                "3",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, replace1, replace2, result);
            result = new[]
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                "0", "1",
                _conflictBlocks.ConflictBlockChanged1,
                "3",
                _conflictBlocks.ConflictBlockChanged2,
                "2",
                _conflictBlocks.ConflictBlockEnd
            };
            TestMerge(source, replace2, replace1, result);
        }
        #endregion

        // Two changes without collision shouldn't yield a conflict.
        // exception: two different additions before same line yield a confilct
        #region without collision
        [TestMethod]
        public void TestAddsOnSame()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0" };
            string[] add1 = { "1", "0" };
            string[] add2 = { "2", "0" };
            string[] result =
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                _conflictBlocks.ConflictBlockChanged1,
                "1",
                _conflictBlocks.ConflictBlockChanged2,
                "2",
                _conflictBlocks.ConflictBlockEnd,
                "0"
            };
            TestMerge(source, add1, add2, result);
            result = new []
            {
                _conflictBlocks.ConflictBlockBegin,
                _conflictBlocks.ConflictBlockSource,
                _conflictBlocks.ConflictBlockChanged1,
                "2",
                _conflictBlocks.ConflictBlockChanged2,
                "1",
                _conflictBlocks.ConflictBlockEnd,
                "0"
            };
            TestMerge(source, add2, add1, result);
        }

        [TestMethod]
        public void TestAdds()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] add1 = { "2", "0", "1" };
            string[] add2 = { "0", "3", "1" };
            string[] result = { "2", "0", "3", "1" };
            TestMerge(source, add1, add2, result, true);
        }

        [TestMethod]
        public void TestAddRemove()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] remove = { "0" };
            string[] result = { "2", "0" };
            TestMerge(source, add, remove, result, true);
        }

        [TestMethod]
        public void TestAddReplace()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] add = { "2", "0", "1" };
            string[] replace = { "0", "3" };
            string[] result = { "2", "0", "3" };
            TestMerge(source, add, replace, result, true);
        }

        [TestMethod]
        public void TestRemoves()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] remove1 = { "1" };
            string[] remove2 = { "0" };
            string[] result = { };
            TestMerge(source, remove1, remove2, result, true);
        }

        [TestMethod]
        public void TestRemoveReplace()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

            string[] source = { "0", "1" };
            string[] remove = { "1" };
            string[] replace = { "0", "2" };
            string[] result = { "2" };
            TestMerge(source, remove, replace, result, true);
        }

        [TestMethod]
        public void TestReplaces()
        {
            if (_merger == null)
            {
                _conflictBlocks = Common.LoadBlocks();
                _merger = new CustomMerger(_conflictBlocks);
            }

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
        #endregion helpers

        private ConflictBlocks _conflictBlocks;
        private CustomMerger _merger;
    }
}