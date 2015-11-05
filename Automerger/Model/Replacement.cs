using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Replacement : Change
    {
        public Replacement(int start, int removedAmount, string[] newContent)
            : base(start, removedAmount, newContent)
        {
            if (removedAmount < 1)
            {
                throw new ArgumentException();
            }
        }
    }
}
