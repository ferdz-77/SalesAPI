using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SalesAPI.Data;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do contexto do banco de dados PostgreSQL
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// Configuração do MongoDB
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
    return mongoClient.GetDatabase("salesdb");
});

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Testar conexão com MongoDB
app.MapGet("/api/v1/test-mongodb", async (IMongoDatabase mongoDatabase) =>
{
    try
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Orders");
        var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty); // Consulta simples
        return Results.Ok(new { Status = "MongoDB connection successful", OrderCount = count });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);

    }
}).WithName("TestMongoDB")
  .WithOpenApi();



// Testar conexão com PostgreSQL
app.MapGet("/api/v1/test-postgresql", async (SalesDbContext db) =>
{
    try
    {
        var count = await db.Orders.CountAsync(); // Faz uma consulta simples
        return Results.Ok(new { Status = "PostgreSQL connection successful", OrderCount = count });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);

    }
}).WithName("TestPostgreSQL")
  .WithOpenApi();



// Exemplo de endpoint de health check
app.MapGet("/api/v1/health", () => Results.Ok(new { Status = "API is running" }))
   .WithName("HealthCheck")
   .WithOpenApi();

app.Run();
