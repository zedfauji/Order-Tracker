using Google.Cloud.Firestore;
using System.Text.Json.Serialization;

namespace RestockMateCloudApi.Models
{
    [FirestoreData] // 🔑 Required for Firestore serialization
    public class ItemDto
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public int Quantity { get; set; }
    }

    [FirestoreData]
    public class OrderDto
    {
        [FirestoreProperty]
        public string EmployeeName { get; set; }

        [FirestoreProperty]
        public List<ItemDto> Items { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }

        [FirestoreProperty]
        public string? SubmittedAt { get; set; }
        public string Id { get; set; } // 🔁 Firestore doc ID

        // 🔧 Optional: parameterless constructor
        public OrderDto() { }
    }
    public class StatusUpdateDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("newStatus")]
        public string NewStatus { get; set; }
    }
}