using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestockMateApp.Models;

namespace RestockMateApp.Forms
{
    public partial class Form1
    {
        private async Task LoadUserManagementTab()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users:\n" + ex.Message);
            }
        }

        private async void addUserButton_Click(object sender, EventArgs e)
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
            MessageBox.Show(response.IsSuccessStatusCode ? "User saved" : "Error saving user");

            await LoadUserManagementTab();
        }

        private async void updateUserButton_Click(object sender, EventArgs e)
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
        }
    }
}