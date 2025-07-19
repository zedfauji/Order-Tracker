# Create project folder
$projectName = "RestockMateCloudApi"
mkdir $projectName
cd $projectName

# Initialize ASP.NET Core Web API project
dotnet new webapi -n $projectName
cd $projectName

# Add sample OrderDto model
$modelsFolder = "Models"
mkdir $modelsFolder
@"
namespace $projectName.Models
{
    public class ItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDto
    {
        public string EmployeeName { get; set; }
        public List<ItemDto> Items { get; set; }
        public string Status { get; set; } // Placed, Confirmed, Ordered
        public string SubmittedAt { get; set; }
    }
}
"@ | Out-File "$modelsFolder\OrderDto.cs"

# Add OrderController
@"
using Microsoft.AspNetCore.Mvc;
using $projectName.Models;

namespace $projectName.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly List<OrderDto> Orders = new();

        [HttpPost("submitOrder")]
        public IActionResult SubmitOrder(OrderDto order)
        {
            order.SubmittedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            Orders.Add(order);
            return Ok(order);
        }

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            return Ok(Orders);
        }

        [HttpPost("updateOrderStatus")]
        public IActionResult UpdateOrder([FromBody] UpdateRequest req)
        {
            var order = Orders.FirstOrDefault(o => o.SubmittedAt == req.SubmittedAt);
            if (order != null)
            {
                order.Status = req.NewStatus;
                return Ok(order);
            }
            return NotFound();
        }

        public class UpdateRequest
        {
            public string SubmittedAt { get; set; }
            public string NewStatus { get; set; }
        }
    }
}
"@ | Out-File "Controllers\OrderController.cs"

# Add Dockerfile
@"
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "$projectName.dll"]
"@ | Out-File "Dockerfile"

Write-Host "`n✅ API Project '$projectName' generated successfully!"
Write-Host "Next: Run 'dotnet build' and test locally, then deploy to Cloud Run 🚀"