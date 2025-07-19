using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestockMateApp
{
    public class CloudOrderClient
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "https://restock-api-904541739138.us-central1.run.app/order/submitOrder"; // 🔁 Replace with your real Cloud Run endpoint

        public static async Task<bool> SubmitOrderAsync(OrderDto order)
        {
            try
            {
                var json = JsonSerializer.Serialize(order);
                Console.WriteLine("📤 Sending JSON:");
                Console.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Cloud response: {responseBody}");
                    return true;
                }
                else
                {
                    Console.WriteLine($" Cloud response error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Exception during Cloud submit: {ex.Message}");
                return false;
            }
        }
    }

    public class ItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDto
    {
        public string EmployeeName { get; set; }
        public List<ItemDto> Items { get; set; }
        public string Status { get; set; } = "Placed";
        public string? SubmittedAt { get; set; } // optional
    }
}