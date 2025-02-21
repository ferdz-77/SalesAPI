using Moq;
using Xunit;
using SalesAPI.Services;
using SalesAPI.Data;
using MongoDB.Driver;
using System.Threading.Tasks;
using SalesAPI.Models;
using SalesAPI.Exceptions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class SalesServiceTests
{
    private readonly DbContextOptions<SalesDbContext> _options;
    private readonly Mock<IMongoDatabase> _mockMongoDb;
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
            QuantidadeEstoque = 5, // Estoque insuficiente
            Nome = "Produto Teste",
            Preco = 50
        };

        // Simulando a busca do produto no MongoDB
        var mockAsyncCursor = new Mock<IAsyncCursor<Produto>>();
        mockAsyncCursor.Setup(_ => _.Current).Returns(new List<Produto> { produtoMock });
        mockAsyncCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                       .Returns(true)
                       .Returns(false);
        mockAsyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);

        _mockProductCollection.Setup(x => x.FindAsync(
            It.IsAny<FilterDefinition<Produto>>(),
            It.IsAny<FindOptions<Produto, Produto>>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockAsyncCursor.Object);

        using var context = new SalesDbContext(_options);
        var salesService = new SalesService(context, _mockMongoDb.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InsufficientStockException>(() =>
            salesService.CreateOrder(productId, quantityRequested));

        Assert.Equal("Estoque insuficiente para o produto.", exception.Message);
    }
}
