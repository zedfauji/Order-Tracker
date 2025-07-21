using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System;
namespace RestockMateApp.Forms
{
    partial class Form1
    {
        private void InitializeHistoryTab()
        {

            this.historyTab = new TabPage();
            this.historyTab.Text = "Order History";
            this.roleBox = new ComboBox();
            this.statusBox = new ComboBox();
            this.roleBox.Location = new Point(10, 540);
            this.roleBox.Size = new Size(150, 25);
            this.roleBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.roleBox.Items.AddRange(new string[] { "Employee", "Administrator" });
            this.roleBox.SelectedIndex = 0;
            this.roleBox.SelectedIndexChanged += new EventHandler(this.roleBox_SelectedIndexChanged);

            this.statusBox.Location = new Point(170, 540);
            this.statusBox.Size = new Size(150, 25);
            this.statusBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.updateStatusButton = new Button();
            this.updateStatusButton.Location = new Point(330, 540);
            this.updateStatusButton.Size = new Size(130, 25);
            this.updateStatusButton.Text = "Update Status";
            this.updateStatusButton.Click += new EventHandler(this.UpdateStatusButton_Click);
        
            this.historyGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(760, 520),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.historyTab.Controls.AddRange(new Control[] {
                historyGridView, roleBox, statusBox, updateStatusButton
            });
        }
    }
}