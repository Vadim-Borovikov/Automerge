using System;
using System.Windows.Forms;

namespace Automerger.Presenter
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

            var merger = new Model.DummyMerger();
            var form = new View.MainForm();
            var presenter = new Presenter(merger, form);

            Application.Run(form);
        }
    }
}
