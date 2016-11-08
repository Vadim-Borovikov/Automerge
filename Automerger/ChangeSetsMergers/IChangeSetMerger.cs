using System.Collections.Generic;
using Automerger.Changes;

namespace Automerger.ChangeSetsMergers
{
    public interface IChangeSetMerger
    {
        IDictionary<int, IChange> Merge(IReadOnlyDictionary<int, IMergableChange> changes1,
                                        IReadOnlyDictionary<int, IMergableChange> changes2,
                                        string[] source);
    }
}
