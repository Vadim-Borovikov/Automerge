using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Automerge;
using Automerge.Changes;
using Automerge.ChangesetsMergers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergeTests
{
    public static class Common
    {
        /// <summary>
        /// Loads the conflict blocks.
        /// </summary>
        /// <returns></returns>
        public static ConflictBlocks ConflictBlocks => new ConflictBlocks
        {
            ConflictBlockBegin =    ConfigurationManager.AppSettings.Get("conflictBlockBegin"),
            ConflictBlockSource =   ConfigurationManager.AppSettings.Get("conflictBlockSource"),
            ConflictBlockChanged1 = ConfigurationManager.AppSettings.Get("conflictBlockChanged1"),
            ConflictBlockChanged2 = ConfigurationManager.AppSettings.Get("conflictBlockChanged2"),
            ConflictBlockEnd =      ConfigurationManager.AppSettings.Get("conflictBlockEnd")
        };

        /// <summary>
        /// Generates the conflict result.
        /// </summary>
        /// <param name="originalBlock">The original block.</param>
        /// <param name="changedBlock1">The changed block1.</param>
        /// <param name="changedBlock2">The changed block2.</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateConflictResult(IEnumerable<string> originalBlock,
                                                                 IEnumerable<string> changedBlock1,
                                                                 IEnumerable<string> changedBlock2)
        {
            yield return ConflictBlocks.ConflictBlockBegin;
            yield return ConflictBlocks.ConflictBlockSource;

            foreach (string line in originalBlock)
            {
                yield return line;
            }

            yield return ConflictBlocks.ConflictBlockChanged1;

            foreach (string line in changedBlock1)
            {
                yield return line;
            }

            yield return ConflictBlocks.ConflictBlockChanged2;

            foreach (string line in changedBlock2)
            {
                yield return line;
            }

            yield return ConflictBlocks.ConflictBlockEnd;
        }

        /// <summary>
        /// Tests the merge.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed1">The first changed content.</param>
        /// <param name="changed2">The second changed content.</param>
        /// <param name="expectedText">The expected text.</param>
        /// <param name="merger">The merger.</param>
        public static void TestMerge(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                                     IReadOnlyList<string> changed2, string[] expectedText, IChangesetMerger merger)
        {
            Result result = Merger.Merge(source, changed1, changed2, merger);
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
        /// <param name="merger">The merger.</param>
        /// <param name="symmetric">Should reversed merge match the original one.</param>
        public static void TestMerge(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                                     IReadOnlyList<string> changed2, string[] expectedText, IChangesetMerger merger,
                                     bool symmetric)
        {
            TestMerge(source, changed1, changed2, expectedText, merger);
            if (symmetric)
            {
                TestMerge(source, changed2, changed1, expectedText, merger);
            }
        }

        /// <summary>
        /// Tests the merge with conflict.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="changed1">The changed1.</param>
        /// <param name="changed2">The changed2.</param>
        /// <param name="merger">The merger.</param>
        public static void TestMergeWithConflict(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                                                 IReadOnlyList<string> changed2, IChangesetMerger merger)
        {
            string[] result = GenerateConflictResult(source, changed1, changed2).ToArray();
            TestMerge(source, changed1, changed2, result, merger);
            result = GenerateConflictResult(source, changed2, changed1).ToArray();
            TestMerge(source, changed2, changed1, result, merger);
        }

        /// <summary>
        /// Tests the null.
        /// </summary>
        /// <param name="merger">The merger.</param>
        public static void TestNull(IChangesetMerger merger)
        {
            string[] empty = { };
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(null, empty, empty, merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, null, empty, merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, null, merger));
            ExceptionAssert.Throws<ArgumentNullException>(() => Merger.Merge(empty, empty, empty, null));
            Merger.Merge(empty, empty, empty, merger);
        }

    }
}