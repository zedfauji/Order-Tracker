using System;
using System.Windows.Forms;

namespace RestockMateApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // Enables default WinForms config
            Application.Run(new Form1()); // Form1 = MainForm
        }
    }
}