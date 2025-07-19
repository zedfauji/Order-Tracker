using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;

namespace RestockMateApp
{
    public static class DBHelper
    {
        private static readonly string dbPath = Path.Combine(Directory.GetParent(Application.StartupPath).Parent.Parent.FullName, "inventory.db");
        private static readonly string connectionString = $"Data Source={dbPath}";

        public static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string createTable = @"
                CREATE TABLE IF NOT EXISTS Items (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ItemName TEXT NOT NULL,
                    Unit TEXT,
                    Category TEXT,
                    DefaultQuantity INTEGER
                );";

            using var command = new SqliteCommand(createTable, connection);
            command.ExecuteNonQuery();
            string createOrderTable = @"
                CREATE TABLE IF NOT EXISTS OrderHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderDate TEXT,
                    Employee TEXT,
                    ItemName TEXT,
                    Quantity INTEGER
                );";

            using var orderCmd = new SqliteCommand(createOrderTable, connection);
            orderCmd.ExecuteNonQuery();

        }


        public static DataTable GetAllItems()
        {
            DataTable table = new DataTable();

            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, ItemName, Unit, Category, DefaultQuantity FROM Items";
                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();

                // Set up table schema
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Item Name", typeof(string));
                table.Columns.Add("Unit", typeof(string));
                table.Columns.Add("Category", typeof(string));
                table.Columns.Add("Default Quantity", typeof(int));
                table.Columns.Add("Order Quantity", typeof(string)); // User entry

                while (reader.Read())
                {
                    var row = table.NewRow();
                    row["Id"] = reader.GetInt32(0);
                    row["Item Name"] = reader.GetString(1);
                    row["Unit"] = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    row["Category"] = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    row["Default Quantity"] = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    row["Order Quantity"] = ""; // Initialize blank
                    table.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Load Error:\n" + ex.Message);
            }


            return table;
        }


        public static void AddItem(string itemName, string unit, string category, int defaultQty)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string insertQuery = @"
                INSERT INTO Items (ItemName, Unit, Category, DefaultQuantity)
                VALUES (@name, @unit, @category, @qty);";

            using var command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@name", itemName);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@qty", defaultQty);
            command.ExecuteNonQuery();
        }

        public static void DeleteItem(int itemId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string deleteQuery = "DELETE FROM Items WHERE Id = @id";
            using var command = new SqliteCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@id", itemId);
            command.ExecuteNonQuery();
        }

        public static void UpdateItem(int itemId, string itemName, string unit, string category, int defaultQty)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string updateQuery = @"
                UPDATE Items
                SET ItemName = @name, Unit = @unit, Category = @category, DefaultQuantity = @qty
                WHERE Id = @id";

            using var command = new SqliteCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@id", itemId);
            command.Parameters.AddWithValue("@name", itemName);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@qty", defaultQty);
            command.ExecuteNonQuery();
        }
        public static void SeedInitialItems()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Check if table already has data
            string countQuery = "SELECT COUNT(*) FROM Items";
            using var countCmd = new SqliteCommand(countQuery, connection);
            long itemCount = (long)countCmd.ExecuteScalar();
            if (itemCount > 0) return; // Already populated

            // Insert starter items
            string insertQuery = @"
                INSERT INTO Items (ItemName, Unit, Category, DefaultQuantity)
                VALUES
                    ('Pizza', 'Piece', 'Food', 10),
                    ('Beer', 'Bottle', 'Beverage', 24),
                    ('Fries', 'Portion', 'Food', 15);";

            using var insertCmd = new SqliteCommand(insertQuery, connection);
            insertCmd.ExecuteNonQuery();
        }
        public static void LogOrderToDatabase(string date, string employee, string itemName, int quantity)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string insert = @"
                INSERT INTO OrderHistory (OrderDate, Employee, ItemName, Quantity)
                VALUES (@date, @employee, @item, @qty);";

            using var command = new SqliteCommand(insert, connection);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@employee", employee);
            command.Parameters.AddWithValue("@item", itemName);
            command.Parameters.AddWithValue("@qty", quantity);
            command.ExecuteNonQuery();
        }
        public static DataTable GetOrderHistory()
        {
            DataTable table = new DataTable();

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT OrderDate, Employee, ItemName, Quantity FROM OrderHistory ORDER BY OrderDate DESC";
            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();

            table.Columns.Add("Date");
            table.Columns.Add("Employee");
            table.Columns.Add("Item");
            table.Columns.Add("Quantity");

            while (reader.Read())
            {
                table.Rows.Add(
                    reader.GetString(0), 
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetInt32(3).ToString()
                );
            }

            return table;
        }
    }
}