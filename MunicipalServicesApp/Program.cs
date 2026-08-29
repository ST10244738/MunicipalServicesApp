using System;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enables Windows visual styles
            Application.EnableVisualStyles();

            // Makes controls use the default Windows text rendering
            Application.SetCompatibleTextRenderingDefault(false);

            // Starts the application with the MainForm
            Application.Run(new MainForm());
        }
    }
}