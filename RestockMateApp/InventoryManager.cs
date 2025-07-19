using System;
using System.Data;
using System.Windows.Forms;

namespace RestockMateApp
{
    public class InventoryManager
    {
        private DataGridView grid;
        private TextBox nameBox, unitBox, categoryBox, qtyBox;
        private Button addButton, updateButton, deleteButton;

        public InventoryManager(DataGridView grid,
                                TextBox nameBox,
                                TextBox unitBox,
                                TextBox categoryBox,
                                TextBox qtyBox,
                                Button addButton,
                                Button updateButton,
                                Button deleteButton)
        {
            this.grid = grid;
            this.nameBox = nameBox;
            this.unitBox = unitBox;
            this.categoryBox = categoryBox;
            this.qtyBox = qtyBox;
            this.addButton = addButton;
            this.updateButton = updateButton;
            this.deleteButton = deleteButton;

            LoadInventory();
            WireEvents();
        }

        public void WireContextMenu(ToolStripMenuItem add, ToolStripMenuItem edit, ToolStripMenuItem delete)
        {
            add.Click += (s, e) => AddViaPrompt();
            edit.Click += (s, e) => EditViaPrompt();
            delete.Click += (s, e) => DeleteSelected();
        }

        private void WireEvents()
        {
            grid.CellValueChanged += Grid_CellValueChanged;
            grid.UserAddedRow += Grid_UserAddedRow;
            grid.KeyDown += Grid_KeyDown;
            grid.SelectionChanged += (s, e) => PopulateFieldsFromSelection();
        }

        private void LoadInventory()
        {
            grid.DataSource = DBHelper.GetAllItems();
            if (grid.Columns["Id"] != null)
            {
                grid.Columns["Id"].ReadOnly = true;
                grid.Columns["Id"].Visible = false;
            }
        }

        private void PopulateFieldsFromSelection()
        {
            if (grid.SelectedRows.Count == 0) return;

            var row = grid.SelectedRows[0];
            nameBox.Text = row.Cells["Item Name"].Value?.ToString();
            unitBox.Text = row.Cells["Unit"].Value?.ToString();
            categoryBox.Text = row.Cells["Category"].Value?.ToString();
            qtyBox.Text = row.Cells["Default Quantity"].Value?.ToString();
        }

        private void AddItem()
        {
            if (!int.TryParse(qtyBox.Text, out int qty)) qty = 0;
            DBHelper.AddItem(nameBox.Text.Trim(), unitBox.Text.Trim(), categoryBox.Text.Trim(), qty);
            LoadInventory();
            ClearFields();
        }

        private void UpdateItem()
        {
            if (grid.SelectedRows.Count == 0) return;
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
            if (!int.TryParse(qtyBox.Text, out int qty)) qty = 0;

            DBHelper.UpdateItem(id, nameBox.Text.Trim(), unitBox.Text.Trim(), categoryBox.Text.Trim(), qty);
            LoadInventory();
            ClearFields();
        }

        private void DeleteItem()
        {
            if (grid.SelectedRows.Count == 0) return;
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
            DBHelper.DeleteItem(id);
            LoadInventory();
            ClearFields();
        }

        private void ClearFields()
        {
            nameBox.Text = "";
            unitBox.Text = "";
            categoryBox.Text = "";
            qtyBox.Text = "";
        }

        private void Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grid.CurrentRow == null || grid.CurrentRow.IsNewRow) return;

            try
            {
                int id = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
                string name = grid.CurrentRow.Cells["Item Name"].Value?.ToString() ?? "";
                string unit = grid.CurrentRow.Cells["Unit"].Value?.ToString() ?? "";
                string category = grid.CurrentRow.Cells["Category"].Value?.ToString() ?? "";
                string qtyStr = grid.CurrentRow.Cells["Default Quantity"].Value?.ToString() ?? "0";
                int qty = int.TryParse(qtyStr, out int result) ? result : 0;

                DBHelper.UpdateItem(id, name, unit, category, qty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed:\n" + ex.Message);
            }
        }

        private void Grid_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            var row = e.Row;
            string name = row.Cells["Item Name"].Value?.ToString() ?? "";
            string unit = row.Cells["Unit"].Value?.ToString() ?? "";
            string category = row.Cells["Category"].Value?.ToString() ?? "";
            string qtyStr = row.Cells["Default Quantity"].Value?.ToString() ?? "0";
            int qty = int.TryParse(qtyStr, out int result) ? result : 0;

            DBHelper.AddItem(name, unit, category, qty);
            LoadInventory(); // Refresh IDs
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && grid.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
                DBHelper.DeleteItem(id);
                LoadInventory();
            }
        }

        private void AddViaPrompt()
        {
            var inputForm = new ItemEditorForm(); // You’ll need to define this form
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                DBHelper.AddItem(inputForm.ItemName, inputForm.Unit, inputForm.Category, inputForm.Quantity);
                LoadInventory();
            }
        }

        private void EditViaPrompt()
        {
            if (grid.SelectedRows.Count == 0) return;
            var row = grid.SelectedRows[0];
            var id = Convert.ToInt32(row.Cells["Id"].Value);
            var inputForm = new ItemEditorForm(
                row.Cells["Item Name"].Value?.ToString(),
                row.Cells["Unit"].Value?.ToString(),
                row.Cells["Category"].Value?.ToString(),
                row.Cells["Default Quantity"].Value?.ToString()
            );

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                DBHelper.UpdateItem(id, inputForm.ItemName, inputForm.Unit, inputForm.Category, inputForm.Quantity);
                LoadInventory();
            }
        }

        private void DeleteSelected()
        {
            if (grid.SelectedRows.Count == 0) return;
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
            DBHelper.DeleteItem(id);
            LoadInventory();
        }
    }
}