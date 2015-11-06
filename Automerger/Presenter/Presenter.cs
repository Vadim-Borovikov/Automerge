using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Automerger.Presenter
{
    public class Presenter
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation
        public Presenter(Model.IChangeSetMerger merger, View.IView view)
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

        public void LoadSource(string path)
        {
            _source = File.ReadAllLines(path);
        }

        public void LoadChanged1(string path)
        {
            _changed1 = File.ReadAllLines(path);
        }

        public void LoadChanged2(string path)
        {
            _changed2 = File.ReadAllLines(path);
        }

        public bool IsReadyForMerge()
        {
            return ((_source != null) && (_changed1 != null) && (_changed2 != null));
        }

        public void Merge()
        {
            Dictionary<int, Model.IMergableChange> changes1 =
                Model.ChangeSetGenerator.Generate(_source, _changed1);
            Dictionary<int, Model.IMergableChange> changes2 =
                Model.ChangeSetGenerator.Generate(_source, _changed2);

            IDictionary<int, Model.IChange> merged = _merger.Merge(changes1, changes2, _source);

            ConflictsDetected = merged.Values.Any(c => c is Model.Conflict);

            var changes = new ReadOnlyDictionary<int, Model.IChange>(merged);

            _result = Model.ChangeSetApplier.Apply(changes, _source);
        }

        public void SaveResult(string path)
        {
            File.WriteAllLines(path, _result);
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Members
        private View.IView _view;
        private Model.IChangeSetMerger _merger;
        private string[] _source;
        private string[] _changed1;
        private string[] _changed2;
        private string[] _result;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
