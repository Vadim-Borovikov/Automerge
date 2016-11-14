using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Changes
{
    public class Conflict : Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Conflict"/> class.
        /// </summary>
        /// <param name="change1">The change1.</param>
        /// <param name="change2">The change2.</param>
        /// <param name="source">The source.</param>
        /// <param name="conflictBlocks">The conflict blocks.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        internal Conflict(IMergableChange change1, IMergableChange change2, IReadOnlyList<string> source,
                          ConflictBlocks conflictBlocks)
        {
            if ((change1 == null) || (change2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            int start = Math.Min(change1.Start, change2.Start);

            if (start > source.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            int afterFinish = Math.Max(change1.AfterFinish, change2.AfterFinish);
            if (afterFinish > source.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((change1.Start >= change2.AfterFinish) || (change2.Start >= change1.AfterFinish))
            {
                if (change1.Start != change2.Start)
                {
                    throw new ArgumentException();
                }
            }

            int removedAmount = afterFinish - start;

            IEnumerable<string> originalBlock = Utils.GetSubArray(source, start, afterFinish);
            IEnumerable<string> changedBlock1 = GetChangedBlock(source, start, afterFinish, change1);
            IEnumerable<string> changedBlock2 = GetChangedBlock(source, start, afterFinish, change2);

            IEnumerable<string> newContent =
                GetNewContent(originalBlock, changedBlock1, changedBlock2, conflictBlocks);

            Initialize(start, removedAmount, newContent.ToArray());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers            
        /// <summary>
        /// Gets the changed block.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="start">The start.</param>
        /// <param name="afterFinish">The after finish.</param>
        /// <param name="change">The change.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetChangedBlock(IReadOnlyList<string> source, int start, int afterFinish,
                                                           IChange change)
        {
            for (int i = start; i < change.Start; ++i)
            {
                yield return source[i];
            }

            foreach (string line in change.NewContent)
            {
                yield return line;
            }

            for (int i = change.Start + change.RemovedAmount; i < afterFinish; ++i)
            {
                yield return source[i];
            }
        }

        /// <summary>
        /// Gets the new content.
        /// </summary>
        /// <param name="originalBlock">The original block.</param>
        /// <param name="changedBlock1">The changed block1.</param>
        /// <param name="changedBlock2">The changed block2.</param>
        /// <param name="conflictBlocks">The conflict blocks.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetNewContent(IEnumerable<string> originalBlock,
                                                         IEnumerable<string> changedBlock1,
                                                         IEnumerable<string> changedBlock2,
                                                         ConflictBlocks conflictBlocks)
        {
            yield return conflictBlocks.ConflictBlockBegin;
            yield return conflictBlocks.ConflictBlockSource;

            foreach (string line in originalBlock)
            {
                yield return line;
            }

            yield return conflictBlocks.ConflictBlockChanged1;

            foreach (string line in changedBlock1)
            {
                yield return line;
            }

            yield return conflictBlocks.ConflictBlockChanged2;

            foreach (string line in changedBlock2)
            {
                yield return line;
            }

            yield return conflictBlocks.ConflictBlockEnd;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
