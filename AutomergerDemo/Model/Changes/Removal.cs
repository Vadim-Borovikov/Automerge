using System;

namespace Automerger.Model
{
    public class Removal : Change, IMergableChange
    {
        public Removal(int start, int amount)
        {
            if ((start < 0) || (amount < 1))
            {
                throw new ArgumentOutOfRangeException();
            }

            Initialize(start, amount, new string[0]);
        }
    }
}
