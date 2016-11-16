using System;
using System.Collections.Generic;
using Automerge.Changes;
using Automerge.Changesets;
using Automerge.ChangesetsMergers;

namespace Automerge
{
    /// <summary>
    /// Merger
    /// </summary>
    public static class Merger
    {
        /// <summary>
        /// Performs the merge.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed1">The first changed content.</param>
        /// <param name="changed2">The second changed content.</param>
        /// <param name="merger">The merger.</param>
        /// <returns></returns>
        public static Result Merge(IReadOnlyList<string> source, IReadOnlyList<string> changed1,
                                   IReadOnlyList<string> changed2, IChangesetMerger merger)
        {
            if (merger == null)
            {
                throw new ArgumentNullException();
            }

            var changeset1 = new MergableChangeset(source, changed1);
            var changeset2 = new MergableChangeset(source, changed2);

            Changeset<IChange> merged = merger.Merge(changeset1, changeset2, source);

            IEnumerable<string> result = merged.Apply(source);

            return new Result(merged, result);
        }
    }
}
