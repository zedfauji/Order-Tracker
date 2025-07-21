
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace RestockMateApp.Forms
{
    public partial class Form1 : Form

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
        private ComboBox roleBox;
        private ComboBox statusBox;
        private Button updateStatusButton;
        private DataGridView userGridView;
        private TextBox usernameBox, passcodeBox;
        private ComboBox roleComboBox;
        private Button addUserButton, updateUserButton, deleteUserButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();

            InitializeOrderTab();

            InitializeHistoryTab();

            InitializeInventoryTab();

            InitializeUserTab();

            InitializeTabControl();

            InitializeFormProperties();


            this.ResumeLayout(false);
        }
    }
}