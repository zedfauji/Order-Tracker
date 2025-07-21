using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RestockMateApp
{
    public class CloudOrderFetcher
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "https://restock-api-904541739138.us-central1.run.app/order/getOrders";

        public static async Task<List<OrderDto>> FetchOrdersAsync()
        {
            try
            {
                var response = await client.GetAsync(apiUrl);
                var responseBody = await response.Content.ReadAsStringAsync();

                //MessageBox.Show(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    var orders = JsonSerializer.Deserialize<List<OrderDto>>(responseBody);
                    return orders ?? new List<OrderDto>();
                }
                else
                {
                    MessageBox.Show($"❌ Status: {response.StatusCode}");
                    return new List<OrderDto>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("🚨 Fetch Exception: " + ex.Message);
                return new List<OrderDto>();
            }
        }
    }
}