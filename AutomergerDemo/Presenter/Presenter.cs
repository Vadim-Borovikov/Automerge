using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Automerger.Changes;
using Automerger.ChangeSets;
using Automerger.ChangeSetsMergers;

namespace AutomergerDemo.Presenter
{
    public class Presenter
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        public Presenter(IChangeSetMerger merger, View.IView view)
        {
            if ((merger == null) || (view == null))
            {
                throw new ArgumentNullException();
            }

            _merger = merger;
            _view = view;

            _view.Initialize(this);
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Public properties and methods
        public bool ConflictsDetected { get; private set; }

        public bool TryLoadSource(string path)
        {
            try
            {
                _source = File.ReadAllLines(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool TryLoadChanged1(string path)
        {
            try
            {
                _changed1 = File.ReadAllLines(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool TryLoadChanged2(string path)
        {
            try
            {
                _changed2 = File.ReadAllLines(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public bool IsReadyForMerge()
        {
            return (_source != null) && (_changed1 != null) && (_changed2 != null);
        }

        public void Merge()
        {
            Dictionary<int, IMergableChange> changes1 =
                ChangeSetGenerator.Generate(_source, _changed1);
            Dictionary<int, IMergableChange> changes2 =
                ChangeSetGenerator.Generate(_source, _changed2);

            IDictionary<int, IChange> merged = _merger.Merge(changes1, changes2, _source);

            ConflictsDetected = merged.Values.Any(c => c is Conflict);

            var changes = new ReadOnlyDictionary<int, IChange>(merged);

            _result = ChangeSetApplier.Apply(changes, _source);
        }

        public bool TrySaveResult(string path)
        {
            try
            {
                WriteAllLines(path, _result);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Helper
        private static void WriteAllLines(string path, string[] lines)
        {
            // Unfortunately File.WriteAllLines leaves an empty line at the end

            string text = string.Join("\r\n", lines);
            File.WriteAllText(path, text);
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Members
        private View.IView _view;
        private IChangeSetMerger _merger;
        private string[] _source;
        private string[] _changed1;
        private string[] _changed2;
        private string[] _result;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
