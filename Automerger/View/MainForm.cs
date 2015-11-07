using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automerger.Presenter;

namespace Automerger.View
{
    public partial class MainForm : Form, IView
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Creation & initialization
        public MainForm()
        {
            InitializeComponent();
        }

        public void Initialize(Presenter.Presenter presenter)
        {
            _presenter = presenter;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Controls' events
        private void MainForm_Load(object sender, EventArgs e)
        {
            labelSource.Text = "";
            labelChanged1.Text = "";
            labelChanged2.Text = "";
            labelMerge.Text = "";

            UpdateControls();
        }

        private void buttonLoadSource_Click(object sender, EventArgs e)
        {
            TryEntrainFile(openFileDialog, _presenter.TryLoadSource, labelSource);
        }

        private void buttonLoadVersion1_Click(object sender, EventArgs e)
        {
            TryEntrainFile(openFileDialog, _presenter.TryLoadChanged1, labelChanged1);
        }

        private void buttonLoadVersion2_Click(object sender, EventArgs e)
        {
            TryEntrainFile(openFileDialog, _presenter.TryLoadChanged2, labelChanged2);
        }

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            _presenter.Merge();

            if (TryEntrainFile(saveFileDialog, _presenter.TrySaveResult, labelMerge))
            {
                toolStripStatusLabel.Text =
                    _presenter.ConflictsDetected ? "Done. Warning: conflict(s) detected!"
                                                 : "Done!";
            }
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Helper
        private void UpdateControls()
        {
            buttonMerge.Enabled = _presenter.IsReadyForMerge();
            toolStripStatusLabel.Text =
                _presenter.IsReadyForMerge() ? "Ready for merge" : "Ready for loading";
        }

        private bool TryEntrainFile(FileDialog dialog, Func<string, bool> tryEntrainFile,
                                    Label label)
        {
            while (dialog.ShowDialog() == DialogResult.OK)
            {
                if (tryEntrainFile(dialog.FileName))
                {
                    label.Text = dialog.FileName;
                    UpdateControls();
                    return true;
                }

                DialogResult result =
                    MessageBox.Show("Sometning went wrong! Please try another file.", "Error",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        #region Member
        private Presenter.Presenter _presenter;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
    }
}
