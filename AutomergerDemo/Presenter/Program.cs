using System;
using System.Windows.Forms;
using Automerge.ChangesetsMergers;
using AutomergeDemo.View;

namespace AutomergeDemo.Presenter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var merger = new CustomMerger(Consts.ConflictBlocks);
            var form = new MainForm();
            var presenter = new Presenter(merger, form);

            Application.Run(form);
        }
    }
}
