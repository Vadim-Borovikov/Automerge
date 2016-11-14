using System.Collections.Generic;
using Automerge.Changes;
using Automerge.Changesets;

namespace Automerge.ChangesetsMergers
{
    /// <summary>
    /// Changeset merger
    /// </summary>
    public interface IChangesetMerger
    {
        /// <summary>
        /// Merges the specified changesets.
        /// </summary>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        Changeset<IChange> Merge(MergableChangeset changeset1, MergableChangeset changeset2,
                                 IReadOnlyList<string> source);
    }
}
