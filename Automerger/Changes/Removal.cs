using System;

namespace Automerger.Changes
{
    /// <summary>
    /// Removal from a file
    /// </summary>
    /// <seealso cref="Automerger.Changes.Change" />
    /// <seealso cref="Automerger.Changes.IMergableChange" />
    public class Removal : Change, IMergableChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Removal"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="amount">The amount.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Removal(int start, int amount)
        {
            if ((start < 0) || (amount < 1))
            {
                throw new ArgumentOutOfRangeException();
            }

            Initialize(start, amount, new string[0]);
        }
    }
}
