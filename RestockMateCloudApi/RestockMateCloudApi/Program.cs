var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Enables controller routing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // 🔑 Adds controller endpoints

app.Run();