using System.Windows.Forms;
using System.Drawing;
using System;
namespace RestockMateApp.Forms
{
    partial class Form1
    {
        private void InitializeTabControl()
        {
            this.tabControl = new TabControl
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600)
            };

            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);

            // Add all tab pages
            this.tabControl.Controls.Add(this.orderTab);
            this.tabControl.Controls.Add(this.historyTab);
            this.tabControl.Controls.Add(this.inventoryTab);
            this.tabControl.Controls.Add(this.userTab);
            // Add to the form
            this.Controls.Clear(); // Optional if replacing existing layout
            this.Controls.Add(this.tabControl);
        }
    }
}