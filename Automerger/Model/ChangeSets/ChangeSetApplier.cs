using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public static class ChangeSetApplier
    {
        public static string[] Apply(IReadOnlyDictionary<int, IChange> changes, string[] source)
        {
            if ((source == null) || (changes == null))
            {
                throw new ArgumentNullException();
            }

            ChangeSetVerifier.Verify(changes, source.Length);

            foreach (int key in changes.Keys)
            {
                if (key + changes[key].RemovedAmount > source.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            var result = new List<string>();

            bool shouldAddSource;
            for (int i = 0; i < source.Length; ++i)
            {
                shouldAddSource = true;

                if (changes.ContainsKey(i))
                {
                    IChange change = changes[i];
                    result.AddRange(change.NewContent);
                    if (change.RemovedAmount > 0)
                    {
                        i = change.AfterFinish - 1;
                        shouldAddSource = false;
                    }
                }

                if (shouldAddSource)
                {
                    result.Add(source[i]);
                }
            }

            return result.ToArray();
        }
    }
}
