using System.Collections.Generic;
using System.Linq;

namespace Automerger.Changes
{
    public abstract class Change : IChange
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Common properties
        public int Start { get; private set; }
        public int AfterFinish { get { return Start + RemovedAmount; } }
        public int RemovedAmount { get; private set; }
        public IReadOnlyList<string> NewContent { get; private set; }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        protected void Initialize(int start, int removedAmount, IReadOnlyList<string> newContent)
        {
            Start = start;
            RemovedAmount = removedAmount;
            NewContent = newContent;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            var o = other as Change;
            return ((Start == o.Start) && (RemovedAmount == o.RemovedAmount) &&
                   NewContent.SequenceEqual(o.NewContent));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Start;
                hash = hash * 11 + RemovedAmount;
                hash = hash * 13 + NewContent.Count();
                foreach (string c in NewContent)
                {
                    hash = hash * 17 + c.GetHashCode();
                }
                return hash;
            }
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
