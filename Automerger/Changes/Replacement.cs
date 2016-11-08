﻿using System;
using System.Collections.Generic;

namespace Automerger.Changes
{
    /// <summary>
    /// Replacement in a file
    /// </summary>
    /// <seealso cref="Automerger.Changes.Change" />
    /// <seealso cref="Automerger.Changes.IMergableChange" />
    public class Replacement : Change, IMergableChange
    {
        public Replacement(int start, int removedAmount, IReadOnlyList<string> newContent)
        {
            if ((start < 0) || (removedAmount < 1))
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

            Initialize(start, removedAmount, newContent);
        }
    }
}
