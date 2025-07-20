using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RestockMateApp
{
    public partial class Form1 : Form
    {
        private string currentUser;
        private string currentRole;

        public Form1(string user, string role)
        {

            try
            {
                currentUser = user;
                currentRole = role;


                InitializeComponent();
                userTab.Visible = currentRole == "Administrator";
/*                MessageBox.Show($"Check: roleBox is null? {(roleBox == null)}\n" +
                                $"addUserButton is null? {(addUserButton == null)}\n" +
                                $"historyGridView is null? {(historyGridView == null)}");
                */
                MessageBox.Show($"Initializing form for {currentUser} with role {currentRole}");

                LoadUserManagementTab();
                MessageBox.Show("userTab is null? " + (userTab == null));
                MessageBox.Show("userTab.Visible: " + userTab.Visible);

                this.Text = $"RestockMate - {DateTime.Now:yyyy-MM-dd}";
                DBHelper.InitializeDatabase();
                DBHelper.SeedInitialItems();
                LoadItemsFromDatabase();

                ContextMenuStrip inventoryContextMenu = new ContextMenuStrip();
                ToolStripMenuItem addMenuItem = new ToolStripMenuItem(" Add New Item");
                ToolStripMenuItem editMenuItem = new ToolStripMenuItem(" Edit Selected");
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem(" Delete Selected");

                inventoryContextMenu.Items.AddRange(new ToolStripItem[] {
                    addMenuItem, editMenuItem, deleteMenuItem
                });
                inventoryGridView.ContextMenuStrip = inventoryContextMenu;

                var manager = new InventoryManager(
                    inventoryGridView, nameBox, unitBox, categoryBox, qtyBox,
                    addButton, updateButton, deleteButton
                );
                manager.WireContextMenu(addMenuItem, editMenuItem, deleteMenuItem);

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

                if (roleBox != null)
                {
                    roleBox.SelectedIndexChanged += roleBox_SelectedIndexChanged;
                    roleBox.SelectedIndex = 0;
                    MessageBox.Show($"Welcome {currentUser} ({currentRole})");
                }
                else
                {
                    MessageBox.Show("Role selection not available. Please check your form design.");
                }

                historyGridView.RowPrePaint += (s, e) =>
                    {
                        if (e.RowIndex < 0) return;

                        var row = historyGridView.Rows[e.RowIndex];
                        var status = row.Cells["Status"].Value?.ToString();

                        if (string.IsNullOrEmpty(status)) return;

                        switch (status)
                        {
                            case "Placed":
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                                break;
                            case "Ordered":
                                row.DefaultCellStyle.BackColor = Color.Khaki;
                                break;
                            case "Received":
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                                break;
                        }
                    };
                UpdateStatusOptions();

                addUserButton.Click += async (s, e) =>
                {
                    var dto = new CreateUserDto
                    {
                        Username = usernameBox.Text.Trim(),
                        Passcode = passcodeBox.Text.Trim(),
                        Role = roleComboBox.SelectedItem?.ToString()
                    };

                    var json = JsonSerializer.Serialize(dto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var client = new HttpClient();
                    var response = await client.PostAsync("https://restock-api-904541739138.us-central1.run.app/order/user/createOrUpdate", content);
                    MessageBox.Show(response.IsSuccessStatusCode ? " User saved" : " Error saving user");

                    await LoadUserManagementTab();
                };
                updateUserButton.Click += async (s, e) =>
                {
                    var selectedRow = userGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
                    if (selectedRow == null)
                    {
                        MessageBox.Show("Please select a user to update.");
                        return;
                    }

                    var username = selectedRow.Cells["username"].Value?.ToString();
                    var dto = new CreateUserDto
                    {
                        Username = usernameBox.Text.Trim(),
                        Passcode = passcodeBox.Text.Trim(),
                        Role = roleComboBox.SelectedItem?.ToString()
                    };

                    var json = JsonSerializer.Serialize(dto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var client = new HttpClient();
                    var response = await client.PostAsync("https://restock-api-904541739138.us-central1.run.app/order/user/createOrUpdate", content);
                    MessageBox.Show(response.IsSuccessStatusCode ? "User updated" : "Error updating user");

                    await LoadUserManagementTab();
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
                            writer.WriteLine($"{orderDate},{employeeName},{item},{quantity}");
                        }
                    }

                    if (itemsToSubmit.Count == 0)
                    {
                        MessageBox.Show("No items to submit.");
                        return;
                    }

                    var orderDto = new OrderDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeName = employeeName,
                        Items = itemsToSubmit,
                        Status = "Placed"
                    };

                    bool success = await CloudOrderClient.SubmitOrderAsync(orderDto);
                    MessageBox.Show(success ? "Order submitted successfully." : "Failed to submit order.");
                }
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
            string supplierNumber = "521234567890";
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
            if (tabControl.SelectedTab == historyTab)
                await RefreshOrderHistoryAsync();

            if (tabControl.SelectedTab == orderTab)
                LoadItemsFromDatabase();
        }

        private async Task RefreshOrderHistoryAsync()
        {
            try
            {
                var orders = await CloudOrderFetcher.FetchOrdersAsync();
                if (orders == null || orders.Count == 0)
                {
                    MessageBox.Show("No orders found in Firestore.");
                    return;
                }

                var table = new DataTable();
                table.Columns.Add("Id");
                table.Columns.Add("Submitted At");
                table.Columns.Add("Employee");
                table.Columns.Add("Status");
                table.Columns.Add("Item");
                table.Columns.Add("Quantity");
                table.Columns.Add("Updated At");

                foreach (var order in orders)
                {
                    if (order?.Items == null || order.Items.Count == 0) continue;

                    foreach (var item in order.Items)
                    {
                        table.Rows.Add(
                            order.Id,
                            order.SubmittedAt ?? "N/A",
                            order.EmployeeName ?? "Unknown",
                            order.Status ?? "Unknown",
                            item.Name ?? "Unnamed",
                            item.Quantity,
                            order.StatusUpdatedAt ?? "—"

                        );
                    }
                }

                historyGridView.AutoGenerateColumns = true;
                historyGridView.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders from Firestore:\n" + ex.Message);
            }
        }

        private void roleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatusOptions();
        }

        private void UpdateStatusOptions()
        {
            statusBox.Items.Clear();
            string role = roleBox.SelectedItem?.ToString();

            if (role == "Administrator")
                statusBox.Items.Add("Ordered");
            else if (role == "Employee")
                statusBox.Items.Add("Received");

            if (statusBox.Items.Count > 0)
                statusBox.SelectedIndex = 0;
        }

        private async void UpdateStatusButton_Click(object sender, EventArgs e)
        {
            if (historyGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to update.");
                return;
            }

            var row = historyGridView.SelectedRows[0];
            string docId = row.Cells["Id"].Value?.ToString();
            string newStatus = statusBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(docId) || string.IsNullOrEmpty(newStatus))
            {
                MessageBox.Show("Invalid selection.");
                return;
            }

            var updateDto = new StatusUpdateDto
            {
                Id = docId,
                NewStatus = newStatus
            };

            bool success = await CloudOrderClient.UpdateOrderStatusAsync(updateDto);
            MessageBox.Show(success ? " Status updated!" : " Failed to update status.");

            await RefreshOrderHistoryAsync();
        }
        private async Task LoadUserManagementTab()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://restock-api-904541739138.us-central1.run.app/order/user/list");
            var json = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var filteredUsers = currentRole == "Administrator"
                ? users
                : users.Where(u => u.username == currentUser).ToList();

            userGridView.DataSource = filteredUsers;
        }
    }
}