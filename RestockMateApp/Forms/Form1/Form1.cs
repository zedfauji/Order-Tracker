using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestockMateApp.Models;
using RestockMateApp.Services;

namespace RestockMateApp.Forms
{
    public partial class Form1 : Form
    {
        // 🔐 Global state
        private string currentUser;
        private string currentRole;

        // 📦 Shared controls
        //        private TabControl tabControl;
//        private TabPage orderTab, historyTab, inventoryTab;

//        private Label employeeLabel, nameLabel, unitLabel, categoryLabel, qtyLabel, passcodeLabel, usernameLabel, roleLabel;
//        private TextBox employeeNameBox, nameBox, unitBox, categoryBox, qtyBox, usernameBox, passcodeBox;
//        private ComboBox roleBox, statusBox, roleComboBox;
//        private Button submitOrderButton, sendWhatsAppButton, addUserButton, updateUserButton, deleteUserButton, addButton, updateButton, deleteButton, updateStatusButton;
//        private DataGridView itemGridView, historyGridView, userGridView, inventoryGridView;
//        private ContextMenuStrip inventoryContextMenu;
//        private ToolStripMenuItem addMenuItem, editMenuItem, deleteMenuItem;

        public Form1(string user, string role)
        {
            InitializeComponent();
            currentUser = user ?? "Unknown";
            currentRole = role ?? "Unknown";

            // ✅ Guard before accessing userTab
            if (userTab != null)
                userTab.Visible = role == "Administrator";
            else
                MessageBox.Show("userTab is not initialized. Role-based UI may not load correctly.");


            this.Text = $"RestockMate - {DateTime.Now:yyyy-MM-dd}";
            this.Load += Form1_Load;
        }
    }
}