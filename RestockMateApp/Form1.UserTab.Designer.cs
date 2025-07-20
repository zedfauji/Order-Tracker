namespace RestockMateApp
{
    partial class Form1
    {
        private void InitializeUserTab()
        {
            this.userTab = new TabPage();
            this.userTab.Text = "User Management";
            this.userTab.Visible = false;

            this.userGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(760, 300),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var usernameLabel = new Label { Text = "Username:", Location = new Point(10, 295), Size = new Size(150, 20) };
            var passcodeLabel = new Label { Text = "Passcode:", Location = new Point(170, 295), Size = new Size(150, 20) };
            var roleLabel = new Label { Text = "Role:", Location = new Point(330, 295), Size = new Size(150, 20) };

            this.usernameBox = new TextBox() { Location = new Point(10, 320), Size = new Size(150, 25) };
            this.passcodeBox = new TextBox() { Location = new Point(170, 320), Size = new Size(150, 25) };
            this.roleComboBox = new ComboBox() { Location = new Point(330, 320), Size = new Size(150, 25) };
            roleComboBox.Items.AddRange(new string[] { "Employee", "Administrator" });
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.addUserButton = new Button() { Text = "Add", Location = new Point(490, 320), Size = new Size(80, 25) };
            this.updateUserButton = new Button() { Text = "Update", Location = new Point(580, 320), Size = new Size(80, 25) };
            this.deleteUserButton = new Button() { Text = "Delete", Location = new Point(670, 320), Size = new Size(80, 25) };

            userTab.Controls.AddRange(new Control[] {
                userGridView, usernameLabel, usernameBox, passcodeLabel, passcodeBox,
                roleLabel, roleComboBox, addUserButton, updateUserButton, deleteUserButton
            });
        }
    }
}