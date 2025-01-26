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

public class SalesServiceTests
{
    private DbContextOptions<SalesDbContext> _options;

    public SalesServiceTests()
    {
        // Configuração do banco de dados em memória para o contexto
        _options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(databaseName: "SalesTestDb")
            .Options;
    }

    [Fact]
    public async Task CreateOrder_Should_ThrowException_When_StockIsInsufficient()
    {
        // Arrange
        var productId = 123;
        var quantityRequested = 10;

        // Usar o banco de dados em memória para o SalesDbContext
        using var context = new SalesDbContext(_options);

        var salesService = new SalesService(context, null);  // Ignorando MongoDB no momento

        // Adicionar um produto com estoque insuficiente
        context.Produtos.Add(new Produto
        {
            ProdutoId = productId,
            QuantidadeEstoque = 5,  // Estoque insuficiente
            Nome = "Produto Exemplo"
        });
        context.SaveChanges();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InsufficientStockException>(() =>
            salesService.CreateOrder(productId, quantityRequested));

        Assert.Equal("Estoque insuficiente para o produto", exception.Message);
    }

    [Fact]
    public async Task CalculateTotal_Should_CalculateCorrectly_With_Discount()
    {
        // Arrange
        var order = new Order
        {
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 123, Quantity = 2, Price = 100 },
                new OrderItem { ProductId = 456, Quantity = 1, Price = 150 }
            },
            Discount = 20
        };

        using var context = new SalesDbContext(_options);

        var salesService = new SalesService(context, null); // Ignorando MongoDB no momento

        // Act
        var total = await salesService.CalculateTotal(order);

        // Assert
        Assert.Equal(330, total); // (2 * 100 + 1 * 150) - 20
    }
}
