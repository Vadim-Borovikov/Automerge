using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model
{
    public static class ChangeSetVerifier
    {
        public static void Verify<T>(IReadOnlyDictionary<int, T> changes, int sourceLength)
            where T : IChange
        {
            if (changes == null)
            {
                throw new ArgumentNullException();
            }

            if (sourceLength < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (int current in changes.Keys)
            {
                if ((current < 0) || (changes[current].AfterFinish > sourceLength))
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (changes[current].Start != current)
                {
                    throw new ArgumentOutOfRangeException();
                }

                foreach (int key in changes.Keys.Where(k => (k > current)))
                {
                    if (key < changes[current].AfterFinish)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
