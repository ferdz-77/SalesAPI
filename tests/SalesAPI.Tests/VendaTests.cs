using System;
using System.Collections.Generic;
using SalesAPI.Models;
using SalesAPI.Services;
using Xunit;
using Moq;
using SalesAPI.Repositories.Interfaces;
using SalesAPI.Data;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class VendaTests
{
    private readonly SalesService _salesService;

    public VendaTests()
    {
        var mockProdutoRepository = new Mock<IProdutoRepository>();
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new SalesDbContext(options);

        _salesService = new SalesService(dbContext, mockProdutoRepository.Object);
    }

    [Fact]
    public void NovaVenda_DeveTerDataVendaDefinida()
    {
        // Arrange & Act
        var venda = new Venda();

        // Assert
        venda.DataVenda.Should().BeAfter(DateTime.MinValue);
    }

    [Fact]
    public void NovaVenda_DeveTerListaDeItensNaoNula()
    {
        // Arrange & Act
        var venda = new Venda();

        // Assert
        venda.VendaItems.Should().NotBeNull();
        venda.VendaItems.Should().BeEmpty();
    }

    [Fact]
    public void CriarVenda_DeveCalcularTotalCorretamente()
    {
        // Arrange
        var venda = new Venda();
        _salesService.AdicionarItem(venda, new VendaItem { Preco = 50.0m, Quantidade = 2 });
        _salesService.AdicionarItem(venda, new VendaItem { Preco = 30.0m, Quantidade = 1 });

        // Act
        decimal totalCalculado = _salesService.CalculateTotal(venda);

        // Assert
        totalCalculado.Should().Be(130.0m);
    }
}
