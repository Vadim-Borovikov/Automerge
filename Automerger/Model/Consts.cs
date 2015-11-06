namespace Automerger.Model
{
    public static class Consts
    {
        public const string CONFLICT_BLOCK_BEGIN = ">>>>> BEGIN OF CONFLICTED BLOCK >>>>>\n";
        public const string CONFLICT_BLOCK_SOURCE = ">>>>> ORIGINAL VERSION          >>>>>\n";
        public const string CONFLICT_BLOCK_CHANGED1 = ">>>>> MODIFIED VERSION 1        >>>>>\n";
        public const string CONFLICT_BLOCK_CHANGED2 = ">>>>> MODIFIED VERSION 2        >>>>>\n";
        public const string CONFLICT_BLOCK_END = ">>>>> END OF CONFLICTED BLOCK   >>>>>\n";
    }
}
