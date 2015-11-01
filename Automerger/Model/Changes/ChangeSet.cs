using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class ChangeSet
    {
        public readonly List<Change> Changes = new List<Change>();

        public ChangeSet(string[] source, string[] changed)
        {
            if ((source == null) || (changed == null))
            {
                throw new ArgumentNullException();
            }

            if (source.Length == 0)
            {
                if (changed.Length == 0)
                {
                    return;
                }

                Changes.Add(new Addition(0, changed));
                return;
            }

            if (changed.Length == 0)
            {
                Changes.Add(new Removal(0, source.Length));
                return;
            }

            string[] newContent = null;
            int removedLinesAmount = 0;

            int i = 0;
            int j = 0;
            while ((i < source.Length) && (j < changed.Length))
            {
                if (AreSame(source[i], changed[j]))
                {
                    ++i;
                    ++j;
                    continue;
                }

                int nextSameInSource = -1;
                int nextSameInChanged = -1;
                for (int i1 = i; i1 < source.Length; ++i1)
                {
                    for (int j1 = j; j1 < changed.Length; ++j1)
                    {
                        if (AreSame(source[i1], changed[j1]))
                        {
                            nextSameInSource = i1;
                            nextSameInChanged = j1;
                        }
                    }
                }

                if (nextSameInSource == i)
                {
                    newContent = GetSubArray(changed, j, nextSameInChanged - 1);
                    Changes.Add(new Addition(i, newContent));
                    ++i;
                    j = nextSameInChanged + 1;
                    continue;
                }

                if (nextSameInChanged == j)
                {
                    removedLinesAmount = nextSameInSource - i;
                    Changes.Add(new Removal(i, removedLinesAmount));
                    i = nextSameInChanged + 1;
                    ++j;
                    continue;
                }

                if (nextSameInSource == -1)
                {
                    nextSameInSource = source.Length;
                    nextSameInChanged = changed.Length;
                }

                removedLinesAmount = nextSameInSource - i;
                newContent = GetSubArray(changed, j, nextSameInChanged - 1);
                Changes.Add(new Replacement(i, newContent, removedLinesAmount));
                i = nextSameInChanged + 1;
                j = nextSameInChanged + 1;
            }
        }

        private static bool AreSame(string a, string b) { return a.Trim().Equals(b.Trim()); }

        private static string[] GetSubArray(string[] source, int left, int right)
        {
            int length = right - left + 1;
            var result = new string[length];
            Array.Copy(source, left, result, 0, length);
            return result;
        }
    }
}
