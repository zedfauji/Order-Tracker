using System;
using System.Windows.Forms;

namespace RestockMateApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // Uses new WinForms features on .NET 6+
            Application.Run(new MainForm());
        }
    }
}