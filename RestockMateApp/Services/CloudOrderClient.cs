using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Windows.Forms;
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

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();


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
        public static async Task<bool> UpdateOrderStatusAsync(StatusUpdateDto update)
        {
            try
            {
                var json = JsonSerializer.Serialize(update);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("https://restock-api-904541739138.us-central1.run.app/order/updateOrderStatus", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($" Status Update Response: {response.StatusCode}");
                Console.WriteLine(responseBody);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Status Update Error: {ex.Message}");
                return false;
            }
        }
    }

    public class ItemDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class OrderDto
    {
        [JsonPropertyName("employeeName")]
        public string EmployeeName { get; set; }
        [JsonPropertyName("items")]
        public List<ItemDto> Items { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; } = "Placed";
        [JsonPropertyName("submittedAt")]
        public string? SubmittedAt { get; set; } // optional
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("statusUpdatedAt")]
        public string StatusUpdatedAt { get; set; }

    }
    public class StatusUpdateDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("newStatus")]
        public string NewStatus { get; set; }
        [JsonPropertyName("statusUpdatedAt")]
        public string StatusUpdatedAt { get; set; }
    }

}