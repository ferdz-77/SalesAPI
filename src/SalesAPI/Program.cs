using Microsoft.EntityFrameworkCore;
using System.Globalization;
using MongoDB.Driver;
using SalesAPI.Models;
using SalesAPI.Data;
using MongoDB.Bson;
using SalesAPI.Repositories.Interfaces;
using SalesAPI.Repositories;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configurar a cultura padrão (ISO 8601 ou outra preferida)
var cultureInfo = new CultureInfo("pt-BR"); // Para formato ISO 8601
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Adicionar serviços ao container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Adicionar serviço de Controllers (para usar MapControllers)
builder.Services.AddControllers();

// Configuração do contexto do banco de dados PostgreSQL
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// Configuração do MongoDB
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
    return mongoClient.GetDatabase("salesdb");  // Banco de dados "salesdb" para MongoDB
});

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeia todos os controllers
app.MapControllers();

// Testar conexão com MongoDB
app.MapGet("/api/v1/test-mongodb", async (IMongoDatabase mongoDatabase) =>
{
    try
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Orders");  // Coleção "Orders"
        var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);  // Consulta simples
        return Results.Ok(new { Status = "MongoDB connection successful", OrderCount = count });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);  // Se ocorrer erro, retorna mensagem de erro
    }
}).WithName("TestMongoDB")
  .WithOpenApi();

// Testar conexão com PostgreSQL
app.MapGet("/api/v1/test-postgresql", async (SalesDbContext db) =>
{
    try
    {
        await db.Database.CanConnectAsync(); // Verifica a conexão
        return Results.Ok(new { Status = "PostgreSQL connection successful" });
    }
    catch (Exception ex)
    {
        // Log da exceção para maior visibilidade
        Console.WriteLine($"Erro ao conectar ao PostgreSQL: {ex.Message}");
        return Results.Problem(detail: ex.Message, statusCode: 500); // Retorna erro
    }
}).WithName("TestPostgreSQL")
  .WithOpenApi();


// Chamar os seeds para popular a base de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); // Obtém o logger

    // Chamar os métodos de seed
    DatabaseSeeder.SeedProdutos(context, logger);  // Passa o logger
                                                   // Seed de Produtos
    DatabaseSeeder.SeedClientes(context);  // Seed de Clientes
}

app.Run();
