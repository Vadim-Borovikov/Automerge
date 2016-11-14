using System;
using System.Configuration;
using System.Windows.Forms;
using Automerge.Changes;
using Automerge.ChangesetsMergers;

namespace AutomergeDemo
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConflictBlocks conflictBlocks = LoadBlocks();
            var merger = new CustomMerger(conflictBlocks);
            var presenter = new Presenter(merger);
            var form = new MainForm(presenter);

            Application.Run(form);
        }

        /// <summary>
        /// Loads the conflict blocks.
        /// </summary>
        /// <returns></returns>
        private static ConflictBlocks LoadBlocks() => new ConflictBlocks
        {
            ConflictBlockBegin    = ConfigurationManager.AppSettings.Get("conflictBlockBegin"),
            ConflictBlockSource   = ConfigurationManager.AppSettings.Get("conflictBlockSource"),
            ConflictBlockChanged1 = ConfigurationManager.AppSettings.Get("conflictBlockChanged1"),
            ConflictBlockChanged2 = ConfigurationManager.AppSettings.Get("conflictBlockChanged2"),
            ConflictBlockEnd      = ConfigurationManager.AppSettings.Get("conflictBlockEnd")
        };
    }
}
