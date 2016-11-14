using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutomergeDemo
{
    /// <summary>
    /// The main form
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class MainForm : Form
    {
        /// <summary>
        /// The presenter
        /// </summary>
        private readonly Presenter _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        internal MainForm(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Controls' events            
        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            labelSource.Text = "";
            labelChanged1.Text = "";
            labelChanged2.Text = "";
            labelMerge.Text = "";

            UpdateControls();
        }

        /// <summary>
        /// Handles the Click event of the buttonLoadSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonLoadSource_Click(object sender, EventArgs e) =>
            TryProcessFile(openFileDialog, _presenter.TryLoadSource, labelSource);

        /// <summary>
        /// Handles the Click event of the buttonLoadVersion1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonLoadVersion1_Click(object sender, EventArgs e) =>
            TryProcessFile(openFileDialog, _presenter.TryLoadChanged1, labelChanged1);

        /// <summary>
        /// Handles the Click event of the buttonLoadVersion2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonLoadVersion2_Click(object sender, EventArgs e) =>
            TryProcessFile(openFileDialog, _presenter.TryLoadChanged2, labelChanged2);

        /// <summary>
        /// Handles the Click event of the buttonMerge control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonMerge_Click(object sender, EventArgs e)
        {
            _presenter.Merge();

            if (!TryProcessFile(saveFileDialog, _presenter.TrySaveResult, labelMerge))
            {
                return;
            }

            toolStripStatusLabel.Text =
                _presenter.ConflictsDetected ? "Done. Warning: conflict(s) detected!" : "Done!";
            Process.Start(saveFileDialog.FileName);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers            
        /// <summary>
        /// Updates the controls.
        /// </summary>
        private void UpdateControls()
        {
            buttonMerge.Enabled = _presenter.IsReadyForMerge();
            toolStripStatusLabel.Text = _presenter.IsReadyForMerge() ? "Ready for merge" : "Ready for loading";
        }

        /// <summary>
        /// Tries to process the file.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <param name="tryProcessFile">The try process file.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        private bool TryProcessFile(FileDialog dialog, Func<string, bool> tryProcessFile, Control label)
        {
            while (dialog.ShowDialog() == DialogResult.OK)
            {
                if (tryProcessFile(dialog.FileName))
                {
                    label.Text = dialog.FileName;
                    UpdateControls();
                    return true;
                }

                DialogResult result = MessageBox.Show(@"Sometning went wrong! Please try another file.", @"Error",
                                                      MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
