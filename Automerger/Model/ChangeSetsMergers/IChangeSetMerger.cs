using System.Collections.Generic;

namespace Automerger.Model
{
    public interface IChangeSetMerger
    {
        IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                        IReadOnlyDictionary<int, IMergableChange> changes2,
                                        int sourceLength);
    }
}
