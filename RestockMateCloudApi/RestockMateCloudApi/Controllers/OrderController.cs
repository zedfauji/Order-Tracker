using Microsoft.AspNetCore.Mvc;
using RestockMateCloudApi.Models;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;

namespace RestockMateCloudApi.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        // In-memory store (temporary)
        private static readonly List<OrderDto> Orders = new();
        FirestoreDb _firestoreDb;

        public OrderController()
        {
            string path = "serviceAccount.json"; // or full path
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            _firestoreDb = FirestoreDb.Create("bola8pos"); // Replace with actual project ID
           
        }


        [HttpPost("submitOrder")]
        public async Task<IActionResult> SubmitOrder([FromBody] OrderDto order)
        {   
            order.SubmittedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            DocumentReference docRef = _firestoreDb.Collection("orders").Document();
            await docRef.SetAsync(order);
            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            QuerySnapshot snapshot = await _firestoreDb.Collection("orders").GetSnapshotAsync();
            var orders = snapshot.Documents.Select(doc => doc.ConvertTo<OrderDto>()).ToList();
            return Ok(orders);
        }

        [HttpPost("updateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] StatusUpdateDto update)
        {
            DocumentReference docRef = _firestoreDb.Collection("orders").Document(update.Id);
            await docRef.UpdateAsync(new Dictionary<string, object>
            {
                { "Status", update.NewStatus },
                { "StatusUpdatedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }
            });
            return Ok(new { success = true });
        }
        [HttpGet("getOrders")]
        public async Task<IActionResult> GetOrders([FromQuery] string? employeeName)
        {
            CollectionReference ordersRef = _firestoreDb.Collection("orders");
            Query query = string.IsNullOrEmpty(employeeName)
                ? ordersRef
                : ordersRef.WhereEqualTo("EmployeeName", employeeName);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            var results = snapshot.Documents.Select(doc => {
                var dto = doc.ConvertTo<OrderDto>();
                dto.Id = doc.Id;
                return dto;
            }).ToList();

            return Ok(results);
        }


        public class UpdateRequest
        {
            public string SubmittedAt { get; set; }
            public string NewStatus { get; set; }
        }
    }
}