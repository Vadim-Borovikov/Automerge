using System.Collections.Generic;

namespace Automerger.Model
{
    public class DummyMerger : IChangeSetMerger
    {
        public IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                               IReadOnlyDictionary<int, IMergableChange> changes2)
        {
            var result = new Dictionary<int, IChange>();

            foreach (KeyValuePair<int, IMergableChange> pair in changes1)
            {
                result.Add(pair.Key, pair.Value);
            }

            return result;
        }
    }
}
