namespace RestockMateApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage orderTab;
        private System.Windows.Forms.TabPage historyTab;
        private System.Windows.Forms.DataGridView itemGridView;
        private System.Windows.Forms.DataGridView historyGridView;
        private System.Windows.Forms.TextBox employeeNameBox;
        private System.Windows.Forms.Label employeeLabel;
        private System.Windows.Forms.Button submitOrderButton;
        private System.Windows.Forms.Button sendWhatsAppButton;
        private System.Windows.Forms.TabPage inventoryTab;
        private System.Windows.Forms.DataGridView inventoryGridView;
        private System.Windows.Forms.TextBox nameBox, unitBox, categoryBox, qtyBox;
        private System.Windows.Forms.Button addButton, updateButton, deleteButton;
        private System.Windows.Forms.Label nameLabel, unitLabel, categoryLabel, qtyLabel;

        private System.Windows.Forms.ContextMenuStrip inventoryContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.itemGridView = new System.Windows.Forms.DataGridView();
            this.employeeNameBox = new System.Windows.Forms.TextBox();
            this.employeeLabel = new System.Windows.Forms.Label();
            this.submitOrderButton = new System.Windows.Forms.Button();
            this.sendWhatsAppButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.itemGridView)).BeginInit();
            this.SuspendLayout();

            // employeeLabel
            this.employeeLabel.Location = new System.Drawing.Point(20, 20);
            this.employeeLabel.Size = new System.Drawing.Size(110, 25);
            this.employeeLabel.Text = "Employee Name:";

            // employeeNameBox
            this.employeeNameBox.Location = new System.Drawing.Point(130, 20);
            this.employeeNameBox.Size = new System.Drawing.Size(150, 27);

            // submitOrderButton
            this.submitOrderButton.Location = new System.Drawing.Point(300, 20);
            this.submitOrderButton.Size = new System.Drawing.Size(130, 30);
            this.submitOrderButton.Text = "Submit Order";
            this.submitOrderButton.Click += new System.EventHandler(this.submitOrderButton_Click);

            // sendWhatsAppButton
            this.sendWhatsAppButton.Location = new System.Drawing.Point(450, 20);
            this.sendWhatsAppButton.Size = new System.Drawing.Size(150, 30);
            this.sendWhatsAppButton.Text = "Send via WhatsApp";
            this.sendWhatsAppButton.Click += new System.EventHandler(this.sendWhatsAppButton_Click);

            // itemGridView
            this.itemGridView.Location = new System.Drawing.Point(20, 70);
            this.itemGridView.Size = new System.Drawing.Size(760, 500);
            this.itemGridView.AllowUserToAddRows = false;
            this.itemGridView.AllowUserToDeleteRows = false;
            this.itemGridView.RowHeadersVisible = false;
            this.itemGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Form1
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.employeeLabel);
            this.Controls.Add(this.employeeNameBox);
            this.Controls.Add(this.submitOrderButton);
            this.Controls.Add(this.sendWhatsAppButton);
            this.Controls.Add(this.itemGridView);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestockMate";
            ((System.ComponentModel.ISupportInitialize)(this.itemGridView)).EndInit();
            this.ResumeLayout(false);

            // TabControl
            this.tabControl = new System.Windows.Forms.TabControl();
            this.orderTab = new System.Windows.Forms.TabPage();
            this.historyTab = new System.Windows.Forms.TabPage();
            this.tabControl.Controls.Add(this.orderTab);
            this.tabControl.Controls.Add(this.historyTab);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);

            // Order Tab
            this.orderTab.Text = "Place Order";
            this.orderTab.Controls.AddRange(new Control[] {
                employeeLabel, employeeNameBox, submitOrderButton, sendWhatsAppButton, itemGridView
            });

            // History Tab
            this.historyTab.Text = "Order History";
            this.historyGridView = new System.Windows.Forms.DataGridView();
            this.historyGridView.Location = new System.Drawing.Point(10, 10);
            this.historyGridView.Size = new System.Drawing.Size(760, 520);
            this.historyGridView.ReadOnly = true;
            this.historyGridView.AllowUserToAddRows = false;
            this.historyGridView.AllowUserToDeleteRows = false;
            this.historyGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.historyTab.Controls.Add(this.historyGridView);

            // Add controls back to form
            this.Controls.Clear();
            this.Controls.Add(this.tabControl);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestockMate";
            this.ClientSize = new System.Drawing.Size(800, 600);
            // Third tab
            this.inventoryTab = new System.Windows.Forms.TabPage();
            this.inventoryTab.Text = "Manage Inventory";

            // Inventory grid
            this.inventoryGridView = new System.Windows.Forms.DataGridView();
            this.inventoryGridView.Location = new System.Drawing.Point(10, 10);
            this.inventoryGridView.Size = new System.Drawing.Size(760, 350);
            this.inventoryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.inventoryGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            inventoryTab.Controls.Add(this.inventoryGridView);

            // Labels + Inputs
            this.nameLabel = new Label() { Text = "Item Name:", Location = new Point(10, 370), Size = new Size(100, 25) };
            this.nameBox = new TextBox() { Location = new Point(110, 370), Size = new Size(150, 25) };
            this.unitLabel = new Label() { Text = "Unit:", Location = new Point(270, 370), Size = new Size(100, 25) };
            this.unitBox = new TextBox() { Location = new Point(320, 370), Size = new Size(100, 25) };
            this.categoryLabel = new Label() { Text = "Category:", Location = new Point(430, 370), Size = new Size(100, 25) };
            this.categoryBox = new TextBox() { Location = new Point(510, 370), Size = new Size(120, 25) };
            this.qtyLabel = new Label() { Text = "Default Qty:", Location = new Point(640, 370), Size = new Size(90, 25) };
            this.qtyBox = new TextBox() { Location = new Point(730, 370), Size = new Size(40, 25) };

            // Buttons
            this.inventoryGridView.Location = new Point(10, 10);
            this.inventoryGridView.Size = new Size(760, 540);
            this.inventoryGridView.AllowUserToAddRows = true;
            this.inventoryGridView.AllowUserToDeleteRows = true;
            this.inventoryGridView.EditMode = DataGridViewEditMode.EditOnEnter;

            inventoryContextMenu = new ContextMenuStrip();
            addMenuItem = new ToolStripMenuItem(" Add New Item");
            editMenuItem = new ToolStripMenuItem(" Edit Selected");
            deleteMenuItem = new ToolStripMenuItem(" Delete Selected");

            inventoryContextMenu.Items.AddRange(new ToolStripItem[] {
                addMenuItem, editMenuItem, deleteMenuItem
            });
            inventoryGridView.ContextMenuStrip = inventoryContextMenu;

            // Add controls to tab
            inventoryTab.Controls.AddRange(new Control[] {
                nameLabel, nameBox, unitLabel, unitBox,
                categoryLabel, categoryBox, qtyLabel, qtyBox
            });

            // Add tab to TabControl
            tabControl.Controls.Add(this.inventoryTab);
        }
    }
}