using System;
using System.Collections.Generic;
using Automerger.Changes;
using Automerger.ChangeSets;

namespace Automerger.ChangeSetsMergers
{
    /// <summary>
    /// Merger that takes all from changes1 and nothing from changes2
    /// </summary>
    public class DummyMerger : IChangeSetMerger
    {
        public IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                               IReadOnlyDictionary<int, IMergableChange> changes2,
                                               string[] source)
        {
            if ((changes1 == null) || (changes2 == null) || (source == null))
            {
                throw new ArgumentNullException();
            }

            ChangeSetVerifier.Verify(changes1, source.Length);
            ChangeSetVerifier.Verify(changes2, source.Length);

            var result = new Dictionary<int, IChange>();

            foreach (int key in changes1.Keys)
            {
                result.Add(key, changes1[key]);
            }

            return result;
        }
    }
}
