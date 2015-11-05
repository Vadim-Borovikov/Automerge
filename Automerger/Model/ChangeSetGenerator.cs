using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    public static class ChangeSetGenerator
    {
        #region Creation
        public static Dictionary<int, IMergableChange> Generate(string[] source, string[] changed)
        {
            if ((source == null) || (changed == null))
            {
                throw new ArgumentNullException();
            }

            _source = source;
            _changed = changed;
            _changes = new Dictionary<int, IMergableChange>();

            AddChanges();

            return _changes;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Scanning & comparing
        private static void AddChanges()
        {
            if (_source.Length == 0)
            {
                if (_changed.Length == 0)
                {
                    return;
                }

                _changes.Add(0, new Addition(0, _changed));
                return;
            }

            if (_changed.Length == 0)
            {
                _changes.Add(0, new Removal(0, _source.Length));
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
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Helpers
        private static void FindNextSame(int startLineInSource, int startLineInChanges,
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

        private static void AddAddition(int dest, int start, int finish)
        {
            string[] content = GetSubArray(_changed, start, finish - 1);
            _changes.Add(dest, new Addition(dest, content));
        }

        private static void AddRemoval(int start, int finish)
        {
            int amount = finish - start;
            _changes.Add(start, new Removal(start, amount));
        }

        private static void AddReplacement(int removalStart, int removalFinish,
                                    int additionStart, int additionFinish)
        {
            int removedAmount = removalFinish - removalStart;
            string[] newContent = GetSubArray(_changed, additionStart, additionFinish - 1);
            _changes.Add(removalStart, new Replacement(removalStart, removedAmount, newContent));
        }

        private static bool AreSame(string a, string b) { return a.Trim().Equals(b.Trim()); }

        private static string[] GetSubArray(string[] source, int left, int right)
        {
            int length = right - left + 1;
            var result = new string[length];
            Array.Copy(source, left, result, 0, length);
            return result;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Members
        private static string[] _source;
        private static string[] _changed;
        private static Dictionary<int, IMergableChange> _changes;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
