using System;
using System.Collections.Generic;

namespace Automerger.Changes
{
    /// <summary>
    /// Addition to a file
    /// </summary>
    /// <seealso cref="Automerger.Changes.Change" />
    /// <seealso cref="Automerger.Changes.IMergableChange" />
    public class Addition : Change, IMergableChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Addition"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="newContent">The new content.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public Addition(int start, IReadOnlyList<string> newContent)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            if (newContent.Count == 0)
            {
                throw new ArgumentException();
            }

            Initialize(start, 0, newContent);
        }
    }
}
