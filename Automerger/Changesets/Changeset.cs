﻿using System;
using System.Collections.Generic;
using System.Linq;
using Automerger.Changes;

namespace Automerger.Changesets
{
    /// <summary>
    /// Changeset
    /// </summary>
    /// <seealso>
    ///     <cref>Dictionary{int, IChange}</cref>
    /// </seealso>
    public class Changeset<T> : Dictionary<int, T>
        where T : IChange
    {
        /// <summary>
        /// Verifies the changeset with respect to specified source length.
        /// </summary>
        /// <param name="sourceLength">Length of the source.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// </exception>
        internal void Verify(int sourceLength)
        {
            if (sourceLength < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (int current in Keys)
            {
                if (current < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                IChange change = this[current];
                if ((change.Start != current) || (change.AfterFinish > sourceLength))
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (Keys.Where(k => k > current).Any(k => k < change.AfterFinish))
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Applies the changeset to the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        internal IEnumerable<string> Apply(IReadOnlyList<string> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            Verify(source.Count);

            if (Keys.Any(k => (k + this[k].RemovedAmount) > source.Count))
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i <= source.Count; ++i)
            {
                bool shouldAddSource = i < source.Count;

                if (ContainsKey(i))
                {
                    IChange change = this[i];

                    foreach (string line in change.NewContent)
                    {
                        yield return line;
                    }

                    if (change.RemovedAmount > 0)
                    {
                        i = change.AfterFinish - 1;
                        shouldAddSource = false;
                    }
                }

                if (shouldAddSource)
                {
                    yield return source[i];
                }
            }
        }
    }
}
