using System;

namespace Automerger.Model
{
    public class Replacement : Change, IMergableChange
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
