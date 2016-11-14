using System;
using System.Collections.Generic;
using System.Linq;
using Automerger.Changes;

namespace Automerger.Changesets
{
    /// <summary>
    /// Changeset that can be merged with another one
    /// </summary>
    /// <seealso>
    ///     <cref>Dictionary{int, IMergableChange}</cref>
    /// </seealso>
    public class MergableChangeset : Changeset<IMergableChange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MergableChangeset"/> class.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed">The changed content.</param>
        internal MergableChangeset(IReadOnlyList<string> source, IReadOnlyList<string> changed)
        {
            IEnumerable<IMergableChange> changes = ExtractChanges(source, changed);

            foreach (IMergableChange change in changes)
            {
                Add(change.Start, change);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Changes creators
        /// <summary>
        /// Creates the addition.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="start">The start.</param>
        /// <param name="afterFinish">The after finish.</param>
        /// <param name="changed">The changed.</param>
        private static Addition CreateAddition(int dest, int start, int afterFinish, IReadOnlyList<string> changed)
        {
            string[] newContent = Utils.GetSubArray(changed, start, afterFinish).ToArray();
            return new Addition(dest, newContent);
        }

        /// <summary>
        /// Creates the removal.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="afterFinish">The after finish.</param>
        private static Removal CreateRemoval(int start, int afterFinish) => new Removal(start, afterFinish - start);

        /// <summary>
        /// Creates the replacement.
        /// </summary>
        /// <param name="additionStart">The addition start.</param>
        /// <param name="afterAdditionFinish">The after addition finish.</param>
        /// <param name="removalStart">The removal start.</param>
        /// <param name="afterRemovalFinish">The after removal finish.</param>
        /// <param name="changed">The changed.</param>
        private static Replacement CreateReplacement(int additionStart, int afterAdditionFinish, int removalStart,
                                                     int afterRemovalFinish, IReadOnlyList<string> changed)
        {
            string[] newContent = Utils.GetSubArray(changed, additionStart, afterAdditionFinish).ToArray();
            int removedAmount = afterRemovalFinish - removalStart;
            return new Replacement(removalStart, removedAmount, newContent);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers        
        /// <summary>
        /// Extracts the changes.
        /// </summary>
        /// <param name="source">The source content.</param>
        /// <param name="changed">The changed content.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        private static IEnumerable<IMergableChange> ExtractChanges(IReadOnlyList<string> source,
                                                                   IReadOnlyList<string> changed)
        {
            if ((source == null) || (changed == null))
            {
                throw new ArgumentNullException();
            }

            if (source.Count == 0)
            {
                if (changed.Count > 0)
                {
                    yield return new Addition(0, changed);
                }

                yield break;
            }

            if (changed.Count == 0)
            {
                yield return new Removal(0, source.Count);
                yield break;
            }

            int i = 0;
            int j = 0;
            for (; (i < source.Count) && (j < changed.Count); ++i, ++j)
            {
                if (AreSame(source[i], changed[j]))
                {
                    continue;
                }

                Positions nextSame = FindNextSame(i, j, source, changed);

                if (nextSame.InSource == i)
                {
                    yield return CreateAddition(i, j, nextSame.InChanged, changed);
                    j = nextSame.InChanged;
                }
                else if (nextSame.InChanged == j)
                {
                    yield return CreateRemoval(i, nextSame.InSource);
                    i = nextSame.InSource;
                }
                else
                {
                    yield return CreateReplacement(j, nextSame.InChanged, i, nextSame.InSource, changed);
                    i = nextSame.InSource;
                    j = nextSame.InChanged;
                }
            }

            if (i >= source.Count)
            {
                if (j < changed.Count)
                {
                    yield return CreateAddition(source.Count, j, changed.Count, changed);
                }
            }
            else
            {
                yield return CreateRemoval(i, source.Count);
            }
        }

        /// <summary>
        /// Finds the next same position.
        /// </summary>
        /// <param name="startInSource">The start in source.</param>
        /// <param name="startInChanges">The start in changes.</param>
        /// <param name="source">The source.</param>
        /// <param name="changed">The changed.</param>
        /// <returns></returns>
        private static Positions FindNextSame(int startInSource, int startInChanges, IReadOnlyList<string> source,
                                              IReadOnlyList<string> changed)
        {
            for (int i = startInSource; i < source.Count; ++i)
            {
                if (string.IsNullOrWhiteSpace(source[i]))
                {
                    continue;
                }

                for (int j = startInChanges; j < changed.Count; ++j)
                {
                    if (AreSame(source[i], changed[j]))
                    {
                        return new Positions(i, j);
                    }
                }
            }
            return new Positions(source.Count, changed.Count);
        }

        /// <summary>
        /// Determines whether the trimmed versions of the specified strings are same
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        /// <returns>
        ///   <c>true</c> the trimmed versions of the specified strings are same; otherwise, <c>false</c>.
        /// </returns>
        private static bool AreSame(string a, string b) => a.Trim().Equals(b.Trim());
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
