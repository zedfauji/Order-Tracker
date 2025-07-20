using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace RestockMateApp
{
    public partial class LoginForm : Form
    {
        private TextBox usernameBox;
        private TextBox passcodeBox;
        private Button loginButton;
        private Label usernameLabel;
        private Label passcodeLabel;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - RestockMate";
            this.Size = new System.Drawing.Size(300, 220);
            this.StartPosition = FormStartPosition.CenterScreen;

            usernameLabel = new Label() { Text = "Username:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(80, 25) };
            usernameBox = new TextBox() { Location = new System.Drawing.Point(110, 20), Size = new System.Drawing.Size(150, 25) };

            passcodeLabel = new Label() { Text = "Passcode:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(80, 25) };
            passcodeBox = new TextBox() { Location = new System.Drawing.Point(110, 60), Size = new System.Drawing.Size(150, 25), UseSystemPasswordChar = true };

            loginButton = new Button() { Text = "Log In", Location = new System.Drawing.Point(110, 100), Size = new System.Drawing.Size(150, 30) };
            loginButton.Click += new EventHandler(LoginButton_Click);

            this.Controls.AddRange(new Control[] { usernameLabel, usernameBox, passcodeLabel, passcodeBox, loginButton });
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text.Trim();
            string passcode = passcodeBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passcode))
            {
                MessageBox.Show("Please enter both username and passcode.");
                return;
            }

            var loginDto = new
            {
                username = username,
                passcode = passcode
            };

            try
            {
                var client = new HttpClient();
                string apiUrl = "https://restock-api-904541739138.us-central1.run.app/order/user/login";
                var json = JsonSerializer.Serialize(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Login failed. Check credentials.");
                    return;
                }

                var result = JsonSerializer.Deserialize<LoginResponseDto>(responseBody);
                if (result != null && result.success)
                {
                    MessageBox.Show($"Welcome {username} ({result.role})");
                    this.Hide();
                    var mainForm = new Form1(username, result.role);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show(); // ✅ Correct way to show second form


                }
                else
                {
                    MessageBox.Show("Invalid credentials or role.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error:\n" + ex.Message);
            }
        }
    }

    public class LoginResponseDto
    {
        public bool success { get; set; }
        public string role { get; set; }
    }
}