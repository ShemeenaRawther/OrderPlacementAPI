var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.MapGet("/api/health", () =>
    Results.Ok(new
    {
        Status = "Healthy",
        Application = "StoreApi"
    }));

app.MapGet("/api/products", () =>
{
    return Results.Ok(new[]
    {
        new { Id = 1, Name = "Laptop", Price = 75000 },
        new { Id = 2, Name = "Mouse", Price = 1500 }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
