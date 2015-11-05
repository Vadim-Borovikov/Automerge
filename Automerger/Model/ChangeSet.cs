using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automerger.Model
{
    public class ChangeSet
    {
        public readonly Dictionary<int, Change> Changes = new Dictionary<int, Change>();

        public ChangeSet(string[] source, string[] changed)
        {
            if ((source == null) || (changed == null))
            {
                throw new ArgumentNullException();
            }

            _source = source;
            _changed = changed;

            AddChanges();
        }

        private void AddChanges()
        {
            if (_source.Length == 0)
            {
                if (_changed.Length == 0)
                {
                    return;
                }

                Changes.Add(0, new Addition(0, _changed));
                return;
            }

            if (_changed.Length == 0)
            {
                Changes.Add(0, new Removal(0, _source.Length));
                return;
            }

            int i = 0;
            int j = 0;
            for (; (i < _source.Length) && (j < _changed.Length); ++i, ++j)
            {
                if (AreSame(_source[i], _changed[j]))
                {
                    continue;
                }

                int nextSameInSource;
                int nextSameInChanged;
                FindNextSame(i, j, out nextSameInSource, out nextSameInChanged);

                if (nextSameInSource == i)
                {
                    AddAddition(i, j, nextSameInChanged);
                    j = nextSameInChanged;
                }
                else if (nextSameInChanged == j)
                {
                    AddRemoval(i, nextSameInSource);
                    i = nextSameInSource;
                }
                else
                {
                    AddReplacement(i, nextSameInSource, j, nextSameInChanged);
                    i = nextSameInSource;
                    j = nextSameInChanged;
                }
            }

            if (i >= _source.Length)
            {
                if (j < _changed.Length)
                {
                    AddAddition(_source.Length, j, _changed.Length);
                }
            }
            else
            {
                AddRemoval(i, _source.Length);
            }
        }

        private void FindNextSame(int startLineInSource, int startLineInChanges,
                                  out int nextSameInSource, out int nextSameInChanged)
        {
            nextSameInSource = _source.Length;
            nextSameInChanged = _changed.Length;
            for (int i = startLineInSource; i < _source.Length; ++i)
            {
                for (int j = startLineInChanges; j < _changed.Length; ++j)
                {
                    if (AreSame(_source[i], _changed[j]))
                    {
                        nextSameInSource = i;
                        nextSameInChanged = j;
                        return;
                    }
                }
            }
        }

        private void AddAddition(int dest, int start, int finish)
        {
            string[] content = GetSubArray(_changed, start, finish - 1);
            Changes.Add(dest, new Addition(dest, content));
        }

        private void AddRemoval(int start, int finish)
        {
            int amount = finish - start;
            Changes.Add(start, new Removal(start, amount));
        }

        private void AddReplacement(int removalStart, int removalFinish,
                                    int additionStart, int additionFinish)
        {
            int removedLinesAmount = removalFinish - removalStart;
            string[] newContent = GetSubArray(_changed, additionStart, additionFinish - 1);
            Changes.Add(removalStart,
                new Replacement(removalStart, newContent, removedLinesAmount));
        }

        private static bool AreSame(string a, string b) { return a.Trim().Equals(b.Trim()); }

        private static string[] GetSubArray(string[] source, int left, int right)
        {
            int length = right - left + 1;
            var result = new string[length];
            Array.Copy(source, left, result, 0, length);
            return result;
        }

        private string[] _source;
        private string[] _changed;
    }
}
