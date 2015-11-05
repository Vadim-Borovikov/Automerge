using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Removal : Change
    {
        public Removal(int start, int amount) : base(start, amount, new string[0])
        {
            if (amount < 1)
            {
                throw new ArgumentException();
            }
        }
    }
}
