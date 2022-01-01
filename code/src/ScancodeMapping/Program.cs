using System;
using System.Windows.Forms;

namespace ScancodeMapping
{
    public class App
    {
        private static MainForm mainForm;

        //
        // Summary:
        //      Returns an instance of application's main form.
        //
        public static MainForm GetMainForm() { return mainForm; }

        //
        // Summary:
        //
        public App()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }

    public static class Program
    {
        //
        // Summary:
        //      The main entry point for the application.
        //
        [STAThread]
        static void Main()
        {
            new App();
        }

    }
}