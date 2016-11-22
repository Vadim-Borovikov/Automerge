using System.Collections.Generic;
using System.Linq;
using Automerge.Changes;

namespace Automerge.Changesets
{
    /// <summary>
    /// Changes that collides to each other
    /// </summary>
    public class CollidingChanges
    {
        /// <summary>
        /// The changes from changeset 1
        /// </summary>
        internal readonly MergableChangeset ChangesFrom1;
        /// <summary>
        /// The changes from changeset2
        /// </summary>
        internal readonly MergableChangeset ChangesFrom2;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollidingChanges"/> class.
        /// </summary>
        /// <param name="changesFrom1">The changes changeset 1.</param>
        /// <param name="changesFrom2">The changes changeset 2.</param>
        internal CollidingChanges(MergableChangeset changesFrom1, MergableChangeset changesFrom2)
        {
            ChangesFrom1 = changesFrom1;
            ChangesFrom2 = changesFrom2;
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsEmpty() => Count() == 0;

        /// <summary>
        /// Tries the get single change.
        /// </summary>
        /// <returns></returns>
        internal IMergableChange TryGetSingle()
        {
            if (Count() != 1)
            {
                return null;
            }
            return ChangesFrom1.Count > 0 ? ChangesFrom1.Values.First() : ChangesFrom2.Values.First();
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        internal ICollection<int> Keys => ChangesFrom1.Keys.Union(ChangesFrom2.Keys).ToList();

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        private int Count() => ChangesFrom1.Count + ChangesFrom2.Count;
    }
}
