var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Example of a health check endpoint
app.MapGet("/api/v1/health", () => Results.Ok(new { Status = "API is running" }))
   .WithName("HealthCheck")
   .WithOpenApi();

app.Run();
