using System.Configuration;
using Automerge.Changes;

namespace AutomergeTests
{
    public static class Common
    {
        /// <summary>
        /// Loads the conflict blocks.
        /// </summary>
        /// <returns></returns>
        public static ConflictBlocks LoadBlocks() => new ConflictBlocks
        {
            ConflictBlockBegin =    ConfigurationManager.AppSettings.Get("conflictBlockBegin"),
            ConflictBlockSource =   ConfigurationManager.AppSettings.Get("conflictBlockSource"),
            ConflictBlockChanged1 = ConfigurationManager.AppSettings.Get("conflictBlockChanged1"),
            ConflictBlockChanged2 = ConfigurationManager.AppSettings.Get("conflictBlockChanged2"),
            ConflictBlockEnd =      ConfigurationManager.AppSettings.Get("conflictBlockEnd")
        };
    }
}