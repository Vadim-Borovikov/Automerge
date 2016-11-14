namespace Automerger.Changesets
{
    /// <summary>
    /// Positions
    /// </summary>
    internal struct Positions
    {
        /// <summary>
        /// Position in the source file
        /// </summary>
        internal readonly int InSource;
        /// <summary>
        /// Position in the changed file
        /// </summary>
        internal readonly int InChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Positions"/> struct.
        /// </summary>
        /// <param name="inSource">Position in the source file.</param>
        /// <param name="inChanged">Position in the changed file.</param>
        internal Positions(int inSource, int inChanged)
        {
            InSource = inSource;
            InChanged = inChanged;
        }
    }
}
