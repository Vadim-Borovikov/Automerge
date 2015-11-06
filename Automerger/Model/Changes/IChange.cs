using System.Collections.Generic;

namespace Automerger.Model
{
    public interface IChange
    {
        int Start { get; }
        int AfterFinish { get; }
        int RemovedAmount { get; }
        IReadOnlyList<string> NewContent { get; }
    }
}
