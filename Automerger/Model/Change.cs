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
        public virtual int RemovedAmount { get { return 0; } }
        public virtual string[] NewContent { get { return new string[0]; } }

        public int Finish { get { return Start + RemovedAmount; } }

        protected Change(int start)
        {
            if (start < 0)
            {
                throw new ArgumentException();
            }
            Start = start;
        }
    }
}
