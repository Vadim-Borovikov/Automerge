using System;
using System.Windows.Forms;
using Automerger.ChangeSetsMergers;

namespace AutomergerDemo.Presenter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var merger = new CustomMerger();
            var form = new View.MainForm();
            var presenter = new Presenter(merger, form);

            Application.Run(form);
        }
    }
}
