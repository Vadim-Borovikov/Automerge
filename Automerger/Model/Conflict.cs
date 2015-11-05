using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Conflict
    {
        public readonly int Start;
        public readonly int RemovedAmount;
        public readonly string[] NewContent1;
        public readonly string[] NewContent2;

        public Conflict(int start, int removedAmount,
                        string[] newContent1, string[] newContent2)
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
            NewContent1 = newContent1;
            NewContent2 = newContent2;
        }
    }
}
