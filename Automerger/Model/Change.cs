using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public abstract class Change
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Common members and properties
        public int Start;
        public readonly int RemovedAmount;
        public IReadOnlyList<string> NewContent { get { return _newContent.AsReadOnly(); } }

        public int Finish { get { return Start + RemovedAmount; } }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        protected Change(int start, int removedAmount, string[] newContent)
        {
            if ((start < 0) || (removedAmount < 0))
            {
                throw new ArgumentException();
            }

            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            Start = start;
            RemovedAmount = removedAmount;
            _newContent = new List<string>(newContent);
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Members
        private List<string> _newContent;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
