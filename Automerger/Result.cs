using System.Collections.Generic;
using Automerger.Changes;
using Automerger.Changesets;

namespace Automerger
{
    /// <summary>
    /// Merge result
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// The changeset
        /// </summary>
        public readonly Changeset<IChange> Changeset;
        /// <summary>
        /// The text with the changes
        /// </summary>
        public readonly IEnumerable<string> Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="changeset">The changeset.</param>
        /// <param name="text">The text.</param>
        internal Result(Changeset<IChange> changeset, IEnumerable<string> text)
        {
            Changeset = changeset;
            Text = text;
        }
    }
}
