using System.Drawing;
using System.Windows.Forms;
namespace RestockMateApp.Forms
{
    partial class Form1
    {
        private System.Windows.Forms.TabPage userTab;
        private void InitializeUserTab()
        {
            this.userTab = new TabPage();
            this.userTab.Text = "User Management";
            this.userTab.Visible = true;

            this.userGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(760, 300),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };




            Label usernameLabel = new Label
            {
                Text = "Username:",
                Location = new Point(10, 295),
                Size = new Size(150, 20)
            };
            this.usernameBox = new TextBox() { Location = new Point(10, 320), Size = new Size(150, 25) };
            Label passcodeLabel = new Label { Text = "Passcode:", Location = new Point(170, 295), Size = new Size(150, 20) };
            this.passcodeBox = new TextBox() { Location = new Point(170, 320), Size = new Size(150, 25), UseSystemPasswordChar = true };
            Label roleLabel = new Label { Text = "Role:", Location = new Point(330, 295), Size = new Size(150, 20) };

            /*
                        Label usernameLabel = new Label { Text = "Username:", Location = new Point(20, 20), Size = new Size(80, 25) };
                        usernameBox = new TextBox { Location = new Point(110, 20), Size = new Size(150, 25) };

                        Label passcodeLabel = new Label { Text = "Passcode:", Location = new Point(20, 60), Size = new Size(80, 25) };
                        passcodeBox = new TextBox { Location = new Point(110, 60), Size = new Size(150, 25), UseSystemPasswordChar = true };

                        Label roleLabel = new Label { Text = "Role:", Location = new Point(20, 100), Size = new Size(80, 25) };
                        */
            roleComboBox = new ComboBox
            {
                Location = new Point(110, 100),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            roleComboBox.Items.AddRange(new string[] { "Employee", "Administrator" });
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.addUserButton = new Button() { Text = "Add", Location = new Point(490, 320), Size = new Size(80, 25) };
            this.updateUserButton = new Button() { Text = "Update", Location = new Point(580, 320), Size = new Size(80, 25) };
            this.deleteUserButton = new Button() { Text = "Delete", Location = new Point(670, 320), Size = new Size(80, 25) };

            userTab.Controls.AddRange(new Control[] {
                userGridView, usernameLabel, usernameBox, passcodeLabel, passcodeBox,
                roleLabel, roleComboBox, addUserButton, updateUserButton, deleteUserButton
            });

            usernameLabel.Name = "usernameLabel";
            passcodeLabel.Name = "passcodeLabel";
            roleLabel.Name = "roleLabel";
        }
    }
}