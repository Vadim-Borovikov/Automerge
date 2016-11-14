using System.Collections.Generic;

namespace Automerger.Changes
{
    /// <summary>
    /// Change in a file
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// First line number
        /// </summary>
        int Start { get; }

        /// <summary>
        /// The number of the line that is one past last
        /// </summary>
        int AfterFinish { get; }

        /// <summary>
        /// Removed lines amount
        /// </summary>
        int RemovedAmount { get; }

        /// <summary>
        /// New lines
        /// </summary>
        IEnumerable<string> NewContent { get; }
    }
}
