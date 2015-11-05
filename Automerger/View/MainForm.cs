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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _presenter.LoadSource(openFileDialog.FileName);
                labelSource.Text = openFileDialog.FileName;

                UpdateControls();
            }
        }

        private void buttonLoadVersion1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _presenter.LoadChanged1(openFileDialog.FileName);
                labelChanged1.Text = openFileDialog.FileName;

                UpdateControls();
            }
        }

        private void buttonLoadVersion2_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _presenter.LoadChanged2(openFileDialog.FileName);
                labelChanged2.Text = openFileDialog.FileName;

                UpdateControls();
            }
        }

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            labelMerge.Text = saveFileDialog.FileName;

            _presenter.Merge();
            _presenter.SaveResult(saveFileDialog.FileName);

            toolStripStatusLabel.Text =
                _presenter.ConflictsDetected ? "Done. Warning: conflict(s) detected!" : "Done!";
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
