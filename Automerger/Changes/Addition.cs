using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerge.Changes
{
    /// <summary>
    /// Addition to a file
    /// </summary>
    /// <seealso cref="Change" />
    /// <seealso cref="IMergableChange" />
    internal class Addition : Change, IMergableChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Addition"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="newContent">The new content.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        internal Addition(int start, IReadOnlyCollection<string> newContent)
        {
            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            if (!newContent.Any())
            {
                throw new ArgumentException();
            }

            Initialize(start, 0, newContent);
        }
    }
}
