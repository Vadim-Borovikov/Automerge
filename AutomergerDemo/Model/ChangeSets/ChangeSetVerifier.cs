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
                if (current < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                IChange change = changes[current];
                if ((change.Start != current) || (change.AfterFinish > sourceLength))
                {
                    throw new ArgumentOutOfRangeException();
                }

                foreach (int key in changes.Keys.Where(k => (k > current)))
                {
                    if (key < change.AfterFinish)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
