using System;
using System.Collections.Generic;
using System.Linq;
using Automerge.Changes;
using Automerge.Changesets;

namespace Automerge.ChangesetsMergers
{
    /// <summary>
    /// Merge rules:
    /// 1. Two identical changes don't yield a conflict
    /// 2. Two different additions before same line yield a confilct
    /// 3. In other cases two changes without collision don't yield a conflict
    /// 4. Two changes with collision always yield a conflict
    /// </summary>
    public class SimpleMerger : IChangesetMerger
    {
        /// <summary>
        /// The conflict blocks
        /// </summary>
        protected readonly ConflictBlocks ConflictBlocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMerger"/> class.
        /// </summary>
        /// <param name="conflictBlocks">The conflict blocks.</param>
        public SimpleMerger(ConflictBlocks conflictBlocks)
        {
            ConflictBlocks = conflictBlocks;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region IChangeSetMerger implementation            
        /// <summary>
        /// Merges the specified changesets.
        /// </summary>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public Changeset<IChange> Merge(MergableChangeset changeset1, MergableChangeset changeset2,
                                        IReadOnlyList<string> source)
        {
            if ((changeset1 == null) || (changeset2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            changeset1.Verify(source.Count);
            changeset2.Verify(source.Count);

            var result = new Changeset<IChange>();

            bool swapped = false;
            int[] lines = changeset1.Keys.Union(changeset2.Keys).OrderBy(l => l).ToArray();
            foreach (int line in lines)
            {
                if (!changeset1.ContainsKey(line))
                {
                    if (!changeset2.ContainsKey(line))
                    {
                        continue;
                    }

                    Utils.Swap(ref changeset1, ref changeset2);
                    swapped = !swapped;
                }

                IMergableChange currentChange = changeset1[line];
                changeset1.Remove(line);

                IChange newChange = currentChange;

                IMergableChange collidingChange = FindCollision(changeset2, line, currentChange.AfterFinish);
                if (collidingChange != null)
                {
                    changeset2.Remove(collidingChange.Start);

                    if (!collidingChange.Equals(currentChange))
                    {
                        if (swapped)
                        {
                            Utils.Swap(ref currentChange, ref collidingChange);
                        }
                        newChange = ProcessCollision(source, currentChange, collidingChange);
                    }
                }

                result.Add(line, newChange);
            }

            return result;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers
        /// <summary>
        /// Processes the collision.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="change1">The change1.</param>
        /// <param name="change2">The change2.</param>
        /// <returns></returns>
        protected virtual IChange ProcessCollision(IReadOnlyList<string> source, IMergableChange change1,
                                                   IMergableChange change2)
        {
            return new Conflict(change1, change2, source, ConflictBlocks);
        }

        /// <summary>
        /// Finds the collision.
        /// </summary>
        /// <param name="changeset">The changeset.</param>
        /// <param name="start">The first line number.</param>
        /// <param name="afterFinish">The number of the line that is one past last.</param>
        /// <returns></returns>
        private static IMergableChange FindCollision(MergableChangeset changeset, int start, int afterFinish)
        {
            if (changeset.Keys.Contains(start))
            {
                return changeset[start];
            }

            for (int i = start; i < afterFinish; ++i)
            {
                if (changeset.ContainsKey(i))
                {
                    return changeset[i];
                }
            }
            return null;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
