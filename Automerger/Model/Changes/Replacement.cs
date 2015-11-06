using System;

namespace Automerger.Model
{
    public class Replacement : Change, IMergableChange
    {
        public Replacement(int start, int removedAmount, string[] newContent)
        {
            if ((start < 0) || (removedAmount < 1))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            if (newContent.Length == 0)
            {
                throw new ArgumentException();
            }

            Initialize(start, removedAmount, newContent);
        }
    }
}
