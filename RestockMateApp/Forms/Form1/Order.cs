using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RestockMateApp.Models;
using RestockMateApp.Services;

namespace RestockMateApp.Forms
{
    public partial class Form1
    {
        private async void submitOrderButton_Click(object sender, EventArgs e)
        {
            string employeeName = employeeNameBox.Text.Trim();
            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Please enter employee name before submitting.");
                return;
            }

            if (itemGridView.DataSource is not DataTable table)
            {
                MessageBox.Show("No data available to submit.");
                return;
            }

            string orderDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var itemsToSubmit = new List<ItemDto>();

            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "order_history.csv");
            bool fileExists = File.Exists(logPath);

            using (StreamWriter writer = new StreamWriter(logPath, append: true))
            {
                if (!fileExists)
                    writer.WriteLine("Date,Employee,Item Name,Ordered Quantity");

                foreach (DataRow row in table.Rows)
                {
                    string item = row["Item Name"]?.ToString() ?? "";
                    string qtyText = table.Columns.Contains("Order Quantity") ? row["Order Quantity"]?.ToString() ?? "0" : "0";
                    int quantity = int.TryParse(qtyText, out int parsed) ? parsed : 0;

                    if (quantity > 0)
                    {
                        itemsToSubmit.Add(new ItemDto { Name = item, Quantity = quantity });
                        DBHelper.LogOrderToDatabase(orderDate, employeeName, item, quantity);
                        writer.WriteLine($"{orderDate},{employeeName},{item},{quantity}");
                    }
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
        private void LoadItemsFromDatabase()
        {
            try
            {
                DataTable itemsTable = DBHelper.GetAllItems();
                itemGridView.DataSource = itemsTable;

                // Optional UI refinements
                itemGridView.Columns["Id"].Visible = false;
                itemGridView.Columns["Order Quantity"].ReadOnly = false; // allow editing

                itemGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                itemGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load items:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}