using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace RestockMateApp.Forms
{
    public partial class Form1
    {
        private async void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == historyTab)
                await RefreshOrderHistoryAsync();

            if (tabControl.SelectedTab == orderTab)
                LoadItemsFromDatabase();
        }

        private void roleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatusOptions();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DBHelper.InitializeDatabase();
            DBHelper.SeedInitialItems();

            // 1. Set form title
            this.Text = $"RestockMate - Logged in as {currentUser} ({currentRole})";

            // 2. Role-based visibility control
            userTab.Visible = currentRole == "Administrator";
            inventoryTab.Visible = currentRole != "Employee"; // Optional tweak

            // 3. Auto-load order tab content
            if (tabControl.SelectedTab == orderTab)
            {
                LoadItemsFromDatabase();
            }

            // 4. Optionally preload history tab data
            // historyGridView.DataSource = DBHelper.GetOrderHistory(); // Uncomment if wired

            // 5. Set default focus
            itemGridView?.Focus();

            // 6. Greet user
            MessageBox.Show($"Welcome {currentUser}! Your role: {currentRole}", "RestockMate", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 7. Fire async load for user management
            _ = LoadUserManagementTab();
            InventoryManager inventoryManager = new InventoryManager(
            inventoryGridView, nameBox, unitBox, categoryBox, qtyBox,
            addButton, updateButton, deleteButton
            );

        }
    }
}