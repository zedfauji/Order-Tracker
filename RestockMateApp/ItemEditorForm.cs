using System;
using System.Windows.Forms;

namespace RestockMateApp
{
    public partial class ItemEditorForm : Form
    {
        public string ItemName => itemNameBox.Text.Trim();
        public string Unit => unitBox.Text.Trim();
        public string Category => categoryBox.Text.Trim();
        public int Quantity { get; private set; } = 0;

        private TextBox itemNameBox, unitBox, categoryBox, qtyBox;
        private Button okButton, cancelButton;

        public ItemEditorForm(string name = "", string unit = "", string category = "", string qty = "")
        {
            InitializeComponent();

            itemNameBox.Text = name;
            unitBox.Text = unit;
            categoryBox.Text = category;
            qtyBox.Text = qty;
        }

        private void InitializeComponent()
        {
            this.Text = "Item Editor";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            Label nameLabel = new Label() { Text = "Item Name:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 25) };
            itemNameBox = new TextBox() { Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(220, 25) };

            Label unitLabel = new Label() { Text = "Unit:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(100, 25) };
            unitBox = new TextBox() { Location = new System.Drawing.Point(130, 60), Size = new System.Drawing.Size(220, 25) };

            Label categoryLabel = new Label() { Text = "Category:", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(100, 25) };
            categoryBox = new TextBox() { Location = new System.Drawing.Point(130, 100), Size = new System.Drawing.Size(220, 25) };

            Label qtyLabel = new Label() { Text = "Default Quantity:", Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(100, 25) };
            qtyBox = new TextBox() { Location = new System.Drawing.Point(130, 140), Size = new System.Drawing.Size(220, 25) };

            okButton = new Button() { Text = "OK", Location = new System.Drawing.Point(130, 190), Size = new System.Drawing.Size(100, 30) };
            cancelButton = new Button() { Text = "Cancel", Location = new System.Drawing.Point(250, 190), Size = new System.Drawing.Size(100, 30) };

            okButton.Click += OkButton_Click;
            cancelButton.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] {
                nameLabel, itemNameBox, unitLabel, unitBox,
                categoryLabel, categoryBox, qtyLabel, qtyBox,
                okButton, cancelButton
            });
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(qtyBox.Text.Trim(), out int qty) || qty < 0)
            {
                MessageBox.Show("Please enter a valid non-negative quantity.");
                return;
            }

            Quantity = qty;
            this.DialogResult = DialogResult.OK;
        }
    }
}