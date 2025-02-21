using Moq;
using Xunit;
using SalesAPI.Services;
using SalesAPI.Data;
using MongoDB.Driver;
using SalesAPI.Models;
using SalesAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Repositories.Interfaces;

public class SalesServiceTests
{
    private readonly DbContextOptions<SalesDbContext> _options;
    private readonly Mock<IMongoDatabase> _mockMongoDb;
    private readonly Mock<IProdutoRepository> _mockProdutoRepository;
    private readonly Mock<IMongoCollection<Produto>> _mockProductCollection;

    public SalesServiceTests()
    {
        _options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(databaseName: $"SalesTestDb_{System.Guid.NewGuid()}") // Evita conflitos
            .Options;

        // Mock do MongoDB
        _mockMongoDb = new Mock<IMongoDatabase>();
        _mockProductCollection = new Mock<IMongoCollection<Produto>>();

        // Configurar para retornar a coleção mockada
        _mockMongoDb.Setup(db => db.GetCollection<Produto>("Products", null))
                    .Returns(_mockProductCollection.Object);

        // Mock do IProdutoRepository
        _mockProdutoRepository = new Mock<IProdutoRepository>();
    }

    [Fact]
    public async Task CreateOrder_Should_ThrowException_When_StockIsInsufficient()
    {
        // Arrange
        var productId = 123;
        var quantityRequested = 10;

        var produtoMock = new Produto
        {
            ProdutoId = productId,
            QuantidadeEstoque = 5, // Estoque insuficientecd
            Nome = "Produto Teste",
            Preco = 50
        };

        // Simulando a busca do produto no repositório
        _mockProdutoRepository.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(produtoMock);

        using var context = new SalesDbContext(_options);
        var salesService = new SalesService(context, _mockProdutoRepository.Object); // Corrigido para passar o mock do repositório

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InsufficientStockException>(() =>
            salesService.CreateOrder(productId, quantityRequested));

        Assert.Equal("Estoque insuficiente para o produto.", exception.Message);
    }

    [Fact]
    public async Task CreateOrder_Should_CreateOrder_When_StockIsSufficient()
    {
        // Arrange
        var productId = 123;
        var quantityRequested = 2;

        var produtoMock = new Produto
        {
            ProdutoId = productId,
            QuantidadeEstoque = 10, // Estoque suficiente
            Nome = "Produto Teste",
            Preco = 50
        };

        // Simulando a busca do produto no repositório
        _mockProdutoRepository.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(produtoMock);

        using var context = new SalesDbContext(_options);
        var salesService = new SalesService(context, _mockProdutoRepository.Object);

        // Act
        await salesService.CreateOrder(productId, quantityRequested);

        // Assert
        var order = await context.Orders.FirstOrDefaultAsync();
        Assert.NotNull(order);
        Assert.Equal(100, order.TotalAmount);
        Assert.Single(order.OrderItems);
        Assert.Equal(productId, order.OrderItems.First().ProductId);
        Assert.Equal(quantityRequested, order.OrderItems.First().Quantity);
    }


}
