using System.Drawing;
using System.Windows.Forms;
using System;

namespace RestockMateApp.Forms
{
    partial class Form1
    {
        private void InitializeOrderTab()
        {
            this.orderTab = new TabPage();
            this.orderTab.Text = "Place Order";

            this.employeeLabel = new Label();
            this.employeeNameBox = new TextBox();
            this.employeeLabel.Location = new Point(20, 20);
            this.employeeLabel.Size = new Size(110, 25);
            this.employeeLabel.Text = "Employee Name:";

            this.employeeNameBox.Location = new Point(130, 20);
            this.employeeNameBox.Size = new Size(150, 27);

            this.submitOrderButton = new Button();
            this.sendWhatsAppButton = new Button();
            this.submitOrderButton.Location = new Point(300, 20);
            this.submitOrderButton.Size = new Size(130, 30);
            this.submitOrderButton.Text = "Submit Order";
            this.submitOrderButton.Click += new EventHandler(this.submitOrderButton_Click);

            this.sendWhatsAppButton.Location = new Point(450, 20);
            this.sendWhatsAppButton.Size = new Size(150, 30);
            this.sendWhatsAppButton.Text = "Send via WhatsApp";
//            this.sendWhatsAppButton.Click += new EventHandler(this.sendWhatsAppButton_Click);

            this.itemGridView = new DataGridView();
            this.itemGridView.Location = new Point(20, 70);
            this.itemGridView.Size = new Size(760, 500);
            this.itemGridView.AllowUserToAddRows = false;
            this.itemGridView.AllowUserToDeleteRows = false;
            this.itemGridView.RowHeadersVisible = false;
            this.itemGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.orderTab.Controls.AddRange(new Control[] {
                employeeLabel, employeeNameBox,
                submitOrderButton, sendWhatsAppButton,
                itemGridView
            });
        }
    }
}