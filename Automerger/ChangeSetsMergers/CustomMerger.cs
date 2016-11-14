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
    /// 4. Addition vs removal yield a replacement
    /// 5. In other cases two changes with collision yield a conflict
    /// </summary>
    public class CustomMerger : IChangesetMerger
    {
        /// <summary>
        /// The conflict blocks
        /// </summary>
        private readonly ConflictBlocks _conflictBlocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMerger"/> class.
        /// </summary>
        /// <param name="conflictBlocks">The conflict blocks.</param>
        public CustomMerger(ConflictBlocks conflictBlocks)
        {
            _conflictBlocks = conflictBlocks;
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

            for (int line = 0; line <= source.Count; ++line)
            {
                if (!changeset1.ContainsKey(line))
                {
                    if (!changeset2.ContainsKey(line))
                    {
                        continue;
                    }

                    Swap(ref changeset1, ref changeset2);
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
                        IChange mergedChange = TryMerge(currentChange, collidingChange);
                        if (mergedChange != null)
                        {
                            newChange = mergedChange;
                        }
                        else
                        {
                            newChange = new Conflict(currentChange, collidingChange, source, _conflictBlocks);
                        }
                    }
                }

                result.Add(line, newChange);
                line = newChange.AfterFinish - 1;
            }

            return result;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers
        /// <summary>
        /// Swaps the specified MergableChangesets.
        /// </summary>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        private static void Swap(ref MergableChangeset changeset1, ref MergableChangeset changeset2)
        {
            MergableChangeset temp = changeset1;
            changeset1 = changeset2;
            changeset2 = temp;
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

        /// <summary>
        /// Tries the merge.
        /// </summary>
        /// <param name="change1">The change1.</param>
        /// <param name="change2">The change2.</param>
        /// <returns></returns>
        private static IChange TryMerge(IMergableChange change1, IMergableChange change2)
        {
            if (change1.Start != change2.Start)
            {
                return null;
            }

            var addition = Utils.TryCastOneOf<Addition>(change1, change2);
            if (addition == null)
            {
                return null;
            }
            var removal = Utils.TryCastOneOf<Removal>(change1, change2);
            if (removal == null)
            {
                return null;
            }

            return new Replacement(removal.Start, removal.RemovedAmount, addition.NewContent.ToArray());
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
