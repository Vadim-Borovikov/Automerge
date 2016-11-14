using System;
using System.Collections.Generic;
using Automerge.Changes;
using Automerge.Changesets;

namespace Automerge.ChangesetsMergers
{
    /// <summary>
    /// Merger that takes all from changes1 and nothing from changes2
    /// </summary>
    internal class DummyMerger : IChangesetMerger
    {
        /// <summary>
        /// Merges the specified changesets.
        /// </summary>
        /// <param name="changeset1">The changeset1.</param>
        /// <param name="changeset2">The changeset2.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
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

            foreach (int key in changeset1.Keys)
            {
                result.Add(key, changeset1[key]);
            }

            return result;
        }
    }
}
