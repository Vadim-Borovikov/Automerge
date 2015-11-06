using System;
using System.Collections.Generic;

namespace Automerger.Model
{
    /// <summary>
    /// Merger that takes all from changes1 and nothing from changes2
    /// </summary>
    public class DummyMerger : IChangeSetMerger
    {
        public IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                               IReadOnlyDictionary<int, IMergableChange> changes2)
        {
            if ((changes1 == null) || (changes2 == null))
            {
                throw new ArgumentNullException();
            }

            var result = new Dictionary<int, IChange>();

            foreach (int key in changes1.Keys)
            {
                result.Add(key, changes1[key]);
            }

            return result;
        }
    }
}
