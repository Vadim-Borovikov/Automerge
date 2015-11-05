using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public abstract class Change : IChange
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Common members and properties
        public int Start { get; private set; }
        public int RemovedAmount { get; private set; }
        public IReadOnlyList<string> NewContent { get { return _newContent.AsReadOnly(); } }
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
