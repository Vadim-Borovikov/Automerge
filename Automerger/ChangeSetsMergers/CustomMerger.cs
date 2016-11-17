using System.Collections.Generic;
using System.Linq;
using Automerge.Changes;

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
    public class CustomMerger : SimpleMerger
    {
        public CustomMerger(ConflictBlocks conflictBlocks) : base(conflictBlocks) { }

        /// <summary>
        /// Processes the collision.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="change1">The change1.</param>
        /// <param name="change2">The change2.</param>
        /// <returns></returns>
        protected override IChange ProcessCollision(IReadOnlyList<string> source, IMergableChange change1,
                                                    IMergableChange change2)
        {
            IChange mergedChange = TryMerge(change1, change2);
            return mergedChange ?? base.ProcessCollision(source, change1, change2);
        }

        /// <summary>
        /// Tries to merge.
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
    }
}
