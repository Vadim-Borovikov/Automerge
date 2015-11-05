using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public abstract class Change
    {
        public readonly int Start;
        public readonly int RemovedAmount;
        public IReadOnlyList<string> NewContent { get { return _newContent.AsReadOnly(); } }

        public int Finish { get { return Start + RemovedAmount; } }

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

        private List<string> _newContent;
    }
}
