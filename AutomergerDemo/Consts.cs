using Automerge.Changes;

namespace AutomergeDemo
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Consts
    {
        public static readonly ConflictBlocks ConflictBlocks = new ConflictBlocks
        {
            ConflictBlockBegin    = ">>>>> BEGIN OF CONFLICTED BLOCK >>>>>",
            ConflictBlockSource   = ">>>>> ORIGINAL VERSION          >>>>>",
            ConflictBlockChanged1 = ">>>>> MODIFIED VERSION 1        >>>>>",
            ConflictBlockChanged2 = ">>>>> MODIFIED VERSION 2        >>>>>",
            ConflictBlockEnd      = ">>>>> END OF CONFLICTED BLOCK   >>>>>"
        };
    }
}
