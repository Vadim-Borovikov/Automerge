using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public class Addition : Change, IMergableChange
    {
        public Addition(int start, IReadOnlyList<string> newContent)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (newContent == null)
            {
                throw new ArgumentNullException();
            }

            if (newContent.Count == 0)
            {
                throw new ArgumentException();
            }

            Initialize(start, 0, newContent);
        }
    }
}
