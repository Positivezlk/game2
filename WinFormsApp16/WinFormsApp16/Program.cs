using System;
using System.Windows.Forms;

namespace WinFormsApp16
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Database.Initialize();
            Application.Run(new Form1());
        }
    }
}
