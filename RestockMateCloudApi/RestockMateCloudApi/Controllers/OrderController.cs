using Microsoft.AspNetCore.Mvc;
using RestockMateCloudApi.Models;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;


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
            Console.WriteLine($"Received order: {JsonSerializer.Serialize(order)}");
            Console.WriteLine($"Items count: {order.Items?.Count}");
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
            try
            {
                DocumentReference docRef = _firestoreDb.Collection("orders").Document(update.Id);
                await docRef.UpdateAsync(new Dictionary<string, object>
                {
                    { "Status", update.NewStatus },
                    { "StatusUpdatedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }
                });
                return Ok(new { success = true });

            }
            catch (Exception ex)
            {
                Console.WriteLine(" Status update failed: " + ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }

            
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
        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var userRef = _firestoreDb.Collection("users").Document(login.Username);
            var snapshot = await userRef.GetSnapshotAsync();

            if (!snapshot.Exists)
                return Unauthorized(new { success = false, error = "User not found." });

            var storedPass = snapshot.GetValue<string>("passcode");
            if (storedPass != login.Passcode)
                return Unauthorized(new { success = false, error = "Incorrect passcode." });

            var role = snapshot.GetValue<string>("role");
            return Ok(new { success = true, role });
        }
        [HttpPost("user/createOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateUserDto dto)
        {
            var userData = new Dictionary<string, object>
            {
                { "username", dto.Username },
                { "passcode", dto.Passcode },
                { "role", dto.Role }
            };

            await _firestoreDb.Collection("users").Document(dto.Username).SetAsync(userData);
            return Ok(new { success = true });
        }
        [HttpGet("user/list")]
        public async Task<IActionResult> ListUsers()
        {
            var snapshot = await _firestoreDb.Collection("users").GetSnapshotAsync();
            var users = new List<object>();

            foreach (var doc in snapshot.Documents)
            {
                users.Add(new
                {
                    username = doc.Id,
                    role = doc.GetValue<string>("role")
                });
            }

            return Ok(users);
        }
        


        public class UpdateRequest
        {
            public string SubmittedAt { get; set; }
            public string NewStatus { get; set; }
        }
    }
}