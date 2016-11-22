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
            var result = new Changeset<IChange>();
            IEnumerable<IChange> changes = CollectChanges(changeset1, changeset2, source);
            foreach (IChange change in changes)
            {
                result.Add(change.Start, change);
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
        /// Processes the collision.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <returns></returns>
        protected virtual IChange ProcessCollision(IReadOnlyList<string> source, MergableChangeset changeset1,
                                                   MergableChangeset changeset2)
        {
            return new Conflict(source, changeset1, changeset2, ConflictBlocks);
        }

        /// <summary>
        /// Collects the changes.
        /// </summary>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        private IEnumerable<IChange> CollectChanges(MergableChangeset changeset1, MergableChangeset changeset2,
                                                    IReadOnlyList<string> source)
        {
            if ((changeset1 == null) || (changeset2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            changeset1.Verify(source.Count);
            changeset2.Verify(source.Count);

            var lines = new SortedSet<int>(changeset1.Keys.Union(changeset2.Keys));
            var removedLines = new HashSet<int>();
            bool swapped = false;
            foreach (int line in lines.Where(l => !removedLines.Contains(l)))
            {
                if (!changeset1.ContainsKey(line))
                {
                    Utils.Swap(ref changeset1, ref changeset2);
                    swapped = !swapped;
                }

                IMergableChange currentChange = changeset1.Extract(line);
                CollidingChanges collidingChanges = FindCollidingChanges(currentChange, changeset1, changeset2);

                if (collidingChanges.IsEmpty())
                {
                    yield return currentChange;
                    continue;
                }

                IMergableChange collidingChange = collidingChanges.TryGetSingle();
                if (collidingChange != null)
                {
                    removedLines.Add(collidingChange.Start);
                    if (swapped)
                    {
                        Utils.Swap(ref currentChange, ref collidingChange);
                    }
                    yield return ProcessCollision(source, currentChange, collidingChange);
                }
                else
                {
                    foreach (int l in collidingChanges.Keys)
                    {
                        removedLines.Add(l);
                    }
                    collidingChanges.ChangesFrom1.Add(currentChange);
                    yield return swapped
                        ? ProcessCollision(source, collidingChanges.ChangesFrom2, collidingChanges.ChangesFrom1)
                        : ProcessCollision(source, collidingChanges.ChangesFrom1, collidingChanges.ChangesFrom2);
                }
            }
        }

        /// <summary>
        /// Finds the colliding changes.
        /// </summary>
        /// <param name="current">The current change.</param>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <returns></returns>
        private static CollidingChanges FindCollidingChanges(IMergableChange current, MergableChangeset changeset1,
                                                             MergableChangeset changeset2)
        {
            var collidingChangesFrom1 = new MergableChangeset();
            var collidingChangesFrom2 = new MergableChangeset();
            bool swapped = false;
            while (true)
            {
                IMergableChange colliding = FindCollision(changeset2, current.Start, current.AfterFinish);
                if ((colliding == null) || colliding.Equals(current))
                {
                    if (swapped)
                    {
                        Utils.Swap(ref collidingChangesFrom1, ref collidingChangesFrom2);
                    }
                    return new CollidingChanges(collidingChangesFrom1, collidingChangesFrom2);
                }

                collidingChangesFrom2.Add(colliding);
                current = colliding;
                Utils.Swap(ref changeset1, ref changeset2);
                Utils.Swap(ref collidingChangesFrom1, ref collidingChangesFrom2);
                swapped = !swapped;
            }
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
            if (changeset.ContainsKey(start))
            {
                return changeset.Extract(start);
            }

            for (int i = start + 1; i < afterFinish; ++i)
            {
                if (changeset.ContainsKey(i))
                {
                    return changeset.Extract(i);
                }
            }
            return null;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
