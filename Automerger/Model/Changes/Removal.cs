using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class Removal : Change
    {
        public override int LinesAmount { get { return _linesAmount; } }

        public Removal(int line, int amount) : base(line)
        {
            if (amount < 1)
            {
                throw new ArgumentException();
            }

            _linesAmount = amount;
        }

        private readonly int _linesAmount;
    }
}
