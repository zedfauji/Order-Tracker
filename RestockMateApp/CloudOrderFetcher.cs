using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestockMateApp
{
    public class CloudOrderFetcher
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "https://restock-api-904541739138.us-central1.run.app/order/getOrders"; // 🔁 Update with your actual endpoint

        public static async Task<List<OrderDto>> FetchOrdersAsync(string? employeeName = null)
        {
            try
            {
                string url = string.IsNullOrEmpty(employeeName)
                    ? apiUrl
                    : $"{apiUrl}?employeeName={Uri.EscapeDataString(employeeName)}";

                var response = await client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("📥 Firestore Response:");
                Console.WriteLine(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    var orders = JsonSerializer.Deserialize<List<OrderDto>>(responseBody);
                    return orders ?? new List<OrderDto>();
                }
                else
                {
                    Console.WriteLine($"❌ Status: {response.StatusCode}");
                    return new List<OrderDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 Fetch Exception: " + ex.Message);
                return new List<OrderDto>();
            }
        }
    }
}