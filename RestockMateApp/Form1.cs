using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RestockMateApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            {
                InitializeComponent();
                this.Text = $"RestockMate - {DateTime.Now:yyyy-MM-dd}";
                DBHelper.InitializeDatabase();
                DBHelper.SeedInitialItems();
                LoadItemsFromDatabase();

                // Context Menu Setup
                ContextMenuStrip inventoryContextMenu = new ContextMenuStrip();
                ToolStripMenuItem addMenuItem = new ToolStripMenuItem("➕ Add New Item");
                ToolStripMenuItem editMenuItem = new ToolStripMenuItem("✏️ Edit Selected");
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("❌ Delete Selected");

                inventoryContextMenu.Items.AddRange(new ToolStripItem[] {
                    addMenuItem, editMenuItem, deleteMenuItem
                });
                inventoryGridView.ContextMenuStrip = inventoryContextMenu;

                // Inventory manager setup
                var manager = new InventoryManager(
                    inventoryGridView, nameBox, unitBox, categoryBox, qtyBox,
                    addButton, updateButton, deleteButton
                );
                manager.WireContextMenu(addMenuItem, editMenuItem, deleteMenuItem);

                // Enable right-click selection
                inventoryGridView.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        var hit = inventoryGridView.HitTest(e.X, e.Y);
                        if (hit.RowIndex >= 0)
                        {
                            inventoryGridView.ClearSelection();
                            inventoryGridView.Rows[hit.RowIndex].Selected = true;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing form:\n" + ex.Message);
            }
        }

        private void LoadItemsFromDatabase()
        {
            try
            {
                DataTable itemsTable = DBHelper.GetAllItems();
                itemGridView.DataSource = itemsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading from database:\n" + ex.Message);
            }
        }

        private async void submitOrderButton_Click(object sender, EventArgs e)
        {   
            var itemsToSubmit = new List<ItemDto>();
            string employeeName = employeeNameBox.Text.Trim();
            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Please enter employee name before submitting.");
                return;
            }

            if (itemGridView.DataSource is DataTable table)
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "order_history.csv");
                bool fileExists = File.Exists(logPath);

                using (StreamWriter writer = new StreamWriter(logPath, append: true))
                {
                    if (!fileExists)
                        writer.WriteLine("Date,Employee,Item Name,Ordered Quantity");

                    string orderDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    foreach (DataRow row in table.Rows)
                    {
                        string item = row["Item Name"]?.ToString() ?? "";
                        string quantity = row.Table.Columns.Contains("Order Quantity") ? row["Order Quantity"]?.ToString() ?? "0" : "0";
                        int parsedQty = int.TryParse(quantity, out int result) ? result : 0;
                        if (parsedQty > 0)
                        {
                            itemsToSubmit.Add(new ItemDto { Name = item, Quantity = parsedQty });
                            DBHelper.LogOrderToDatabase(orderDate, employeeName, item, parsedQty);
                        }

                        DBHelper.LogOrderToDatabase(orderDate, employeeName, item, parsedQty);

                        if (!string.IsNullOrWhiteSpace(quantity) && quantity != "0")
                            writer.WriteLine($"{orderDate},{employeeName},{item},{quantity}");
                    }
                    var orderDto = new OrderDto
                    {
                        EmployeeName = employeeName,
                        Items = itemsToSubmit,
                        Status = "Placed"
                    };
                    bool success = await CloudOrderClient.SubmitOrderAsync(orderDto);
                    if (!success)
                    {
                        MessageBox.Show("Failed to submit order to cloud.");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Order submitted successfully to cloud.");
                    }
                }

                MessageBox.Show($"✅ Order saved successfully!\n\nFile saved at:\n{logPath}");
            }
            else
            {
                MessageBox.Show("No data available to submit.");
            }
        }

        private void sendWhatsAppButton_Click(object sender, EventArgs e)
        {
            string employeeName = employeeNameBox.Text.Trim();
            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Please enter employee name before sending.");
                return;
            }

            if (!(itemGridView.DataSource is DataTable table))
            {
                MessageBox.Show("No item list loaded.");
                return;
            }

            StringBuilder messageBuilder = new StringBuilder();
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            messageBuilder.AppendLine($"🗓️ Order Date: {date}");
            messageBuilder.AppendLine($"👤 Employee: {employeeName}");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("📦 Order List:");

            foreach (DataRow row in table.Rows)
            {
                string item = row["Item Name"]?.ToString() ?? "";
                string quantity = row.Table.Columns.Contains("Order Quantity") ? row["Order Quantity"]?.ToString() ?? "0" : "0";

                if (!string.IsNullOrWhiteSpace(quantity) && quantity != "0")
                    messageBuilder.AppendLine($"- {item}: {quantity}");
            }

            string message = Uri.EscapeDataString(messageBuilder.ToString());
            string supplierNumber = "521234567890"; // 🔧 Replace with actual supplier number
            string whatsappUrl = $"https://wa.me/{supplierNumber}?text={message}";

            try
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {whatsappUrl}") { CreateNoWindow = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open WhatsApp:\n" + ex.Message);
            }
        }

        private async void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == myOrdersTab)
            {
                string employee = employeeNameBox.Text.Trim();
                var orders = await CloudOrderFetcher.FetchOrdersAsync(); // 🔁 No filter

                var table = new DataTable();
                table.Columns.Add("Submitted At");
                table.Columns.Add("Status");
                table.Columns.Add("Item");
                table.Columns.Add("Quantity");

                foreach (var order in orders)
                {
                    foreach (var item in order.Items)
                    {
                        table.Rows.Add(order.SubmittedAt, order.Status, item.Name, item.Quantity);
                    }
                }

                myOrderGridView.DataSource = table;
            }
        }
    }
}