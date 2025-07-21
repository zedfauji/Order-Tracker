using System.Collections.Generic;

namespace RestockMateApp.Models
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public List<ItemDto> Items { get; set; }
        public string Status { get; set; }
        public string SubmittedAt { get; set; }     // Optional: Date submitted
        public string StatusUpdatedAt { get; set; } // Optional: Last status update
    }
}