using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestockMateApp.Models;
using RestockMateApp.Services;

namespace RestockMateApp.Forms
{
    public partial class Form1
    {
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
            MessageBox.Show(success ? "Status updated!" : "Failed to update status.");

            await RefreshOrderHistoryAsync();
        }
    }
}