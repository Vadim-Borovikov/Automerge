using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Automerge;
using Automerge.Changes;
using Automerge.ChangesetsMergers;

namespace AutomergeDemo
{
    /// <summary>
    /// Works with files and merges
    /// </summary>
    internal class Presenter
    {
        /// <summary>
        /// Gets a value indicating whether conflicts were detected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if conflicts were detected; otherwise, <c>false</c>.
        /// </value>
        internal bool ConflictsDetected => _result.Changeset.Values.Any(c => c is Conflict);

        /// <summary>
        /// Initializes a new instance of the <see cref="Presenter"/> class.
        /// </summary>
        /// <param name="merger">The merger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        internal Presenter(IChangesetMerger merger)
        {
            if (merger == null)
            {
                throw new ArgumentNullException();
            }

            _merger = merger;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public properties and methods            
        /// <summary>
        /// Tries to load the source file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        internal bool TryLoadSource(string path)
        {
            _source = TryLoadFile(path);
            return _source == null;
        }

        /// <summary>
        /// Tries to load the first changed file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        internal bool TryLoadChanged1(string path)
        {
            _changed1 = TryLoadFile(path);
            return _changed1 == null;
        }

        /// <summary>
        /// Tries to load the second changed file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        internal bool TryLoadChanged2(string path)
        {
            _changed2 = TryLoadFile(path);
            return _changed2 == null;
        }

        /// <summary>
        /// Tries to save the result.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        internal bool TrySaveResult(string path)
        {
            return TrySaveFile(path, _result.Text);
        }

        /// <summary>
        /// Determines whether presenter is ready for merge.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if presenter is ready for merge; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsReadyForMerge() => (_source != null) && (_changed1 != null) && (_changed2 != null);

        /// <summary>
        /// Performs the merges.
        /// </summary>
        internal void Merge()
        {
            _result = Merger.Merge(_source, _changed1, _changed2, _merger);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers            
        /// <summary>
        /// Tries to load the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string[] TryLoadFile(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to save the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="lines">The lines.</param>
        /// <returns></returns>
        private static bool TrySaveFile(string path, IEnumerable<string> lines)
        {
            try
            {
                WriteAllLines(path, lines);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Writes all lines. Unfortunately File.WriteAllLines leaves an empty line at the end
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="lines">The lines.</param>
        private static void WriteAllLines(string path, IEnumerable<string> lines)
        {
            string text = string.Join("\r\n", lines);
            File.WriteAllText(path, text);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Members
        private readonly IChangesetMerger _merger;
        private string[] _source;
        private string[] _changed1;
        private string[] _changed2;
        private Result _result;
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
