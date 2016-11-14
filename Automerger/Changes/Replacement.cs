using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerge.Changes
{
    /// <summary>
    /// Replacement in a file
    /// </summary>
    /// <seealso cref="Change" />
    /// <seealso cref="IMergableChange" />
    internal class Replacement : Change, IMergableChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Replacement"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="removedAmount">The removed amount.</param>
        /// <param name="newContent">The new content.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        internal Replacement(int start, int removedAmount, IReadOnlyCollection<string> newContent)
        {
            if (removedAmount < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            if (!newContent.Any())
            {
                throw new ArgumentException();
            }

            Initialize(start, removedAmount, newContent);
        }
    }
}
