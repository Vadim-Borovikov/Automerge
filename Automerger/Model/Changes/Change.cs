using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public abstract class Change
    {
        public readonly int Line;
        public abstract int LinesAmount { get; }

        protected Change(int line)
        {
            if (line < 1)
            {
                throw new ArgumentException();
            }
            Line = line;
        }
    }
}
