using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Removal : Change
    {
        public override int RemovedAmount { get { return _removedAmount; } }

        public Removal(int start, int amount) : base(start)
        {
            if (amount < 1)
            {
                throw new ArgumentException();
            }

            _removedAmount = amount;
        }

        private readonly int _removedAmount;
    }
}
