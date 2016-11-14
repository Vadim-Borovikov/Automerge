using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerge.Changes
{
    /// <summary>
    /// Change in a file
    /// </summary>
    /// <seealso cref="IChange" />
    public abstract class Change : IChange
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region IChange implementation        
        /// <summary>
        /// First line number
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// The number of the line that is one past last
        /// </summary>
        public int AfterFinish => Start + RemovedAmount;

        /// <summary>
        /// Removed lines amount
        /// </summary>
        public int RemovedAmount { get; private set; }

        /// <summary>
        /// New lines
        /// </summary>
        public IEnumerable<string> NewContent { get; private set; }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="Change"/> class.
        /// </summary>
        /// <param name="start">The first line number.</param>
        /// <param name="removedAmount">The removed amount.</param>
        /// <param name="newContent">The new content.</param>
        protected void Initialize(int start, int removedAmount, IEnumerable<string> newContent)
        {
            if ((start < 0) || (removedAmount < 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            Start = start;
            RemovedAmount = removedAmount;
            NewContent = newContent;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (other == this)
            {
                return true;
            }

            if (other.GetHashCode() != GetHashCode())
            {
                return false;
            }

            var o = other as Change;
            if (o == null)
            {
                return false;
            }

            return (Start == o.Start) && (RemovedAmount == o.RemovedAmount) && NewContent.SequenceEqual(o.NewContent);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Start;
                hash = hash * 11 + RemovedAmount;
                foreach (string c in NewContent)
                {
                    hash = hash * 17 + c.GetHashCode();
                }
                return hash;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
