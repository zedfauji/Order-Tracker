using System.Windows.Forms;
using System.Drawing;

namespace RestockMateApp.Forms
{
    partial class Form1
    {
        // Only keep the initialization logic, no field declarations here
        private void InitializeInventoryTab()
        {
            // Initialization logic only; all field declarations are in Form1.Designer.cs
            this.inventoryTab = new TabPage();
            this.inventoryTab.Text = "Manage Inventory";

            this.inventoryGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(760, 540),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = true,
                AllowUserToDeleteRows = true,
                EditMode = DataGridViewEditMode.EditOnEnter
            };
        
            this.inventoryContextMenu = new ContextMenuStrip();
            this.addMenuItem = new ToolStripMenuItem(" Add New Item");
            this.editMenuItem = new ToolStripMenuItem(" Edit Selected");
            this.deleteMenuItem = new ToolStripMenuItem(" Delete Selected");
            this.inventoryContextMenu.Items.AddRange(new ToolStripItem[] {
                addMenuItem, editMenuItem, deleteMenuItem
            });
            this.inventoryGridView.ContextMenuStrip = inventoryContextMenu;

            this.nameLabel = new Label { Text = "Item Name:", Location = new Point(10, 370), Size = new Size(100, 25) };
            this.nameBox = new TextBox { Location = new Point(110, 370), Size = new Size(150, 25) };

            this.unitLabel = new Label { Text = "Unit:", Location = new Point(270, 370), Size = new Size(100, 25) };
            this.unitBox = new TextBox { Location = new Point(320, 370), Size = new Size(100, 25) };

            this.categoryLabel = new Label { Text = "Category:", Location = new Point(430, 370), Size = new Size(100, 25) };
            this.categoryBox = new TextBox { Location = new Point(510, 370), Size = new Size(120, 25) };

            this.qtyLabel = new Label { Text = "Default Qty:", Location = new Point(640, 370), Size = new Size(90, 25) };
            this.qtyBox = new TextBox { Location = new Point(730, 370), Size = new Size(40, 25) };

            this.inventoryTab.Controls.AddRange(new Control[] {
                inventoryGridView,
                nameLabel, nameBox, unitLabel, unitBox,
                categoryLabel, categoryBox, qtyLabel, qtyBox
            });
        }
    }
}