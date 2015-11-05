using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public class Conflict
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Common members
        public readonly int Start;
        public readonly int RemovedAmount;
        public IReadOnlyList<string> NewContent1;
        public IReadOnlyList<string> NewContent2;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        public Conflict(int start, int removedAmount, string[] newContent1, string[] newContent2)
        {
            if ((start < 0) || (removedAmount < 1))
            {
                throw new ArgumentException();
            }
            if ((newContent1 == null) || (newContent2 == null))
            {
                throw new ArgumentNullException();
            }
            if ((newContent1.Length == 0) || (newContent2.Length == 0))
            {
                throw new ArgumentException();
            }

            Start = start;
            RemovedAmount = removedAmount;
            NewContent1 = (new List<string>(newContent1)).AsReadOnly();
            NewContent2 = (new List<string>(newContent2)).AsReadOnly();
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
