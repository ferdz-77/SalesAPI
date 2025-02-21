using System;
using System.Collections.Generic;
using SalesAPI.Models;
using Xunit;

public class VendaTests
{
    [Fact]
    public void NovaVenda_DeveTerDataVendaDefinida()
    {
        // Arrange & Act
        var venda = new Venda();

        // Assert
        Assert.True(venda.DataVenda > DateTime.MinValue);
    }

    [Fact]
    public void NovaVenda_DeveTerListaDeItensNaoNula()
    {
        // Arrange & Act
        var venda = new Venda();

        // Assert
        Assert.NotNull(venda.VendaItems);
    }

    [Fact]
    public void CriarVenda_DeveCalcularTotalCorretamente()
    {
        // Arrange
        var venda = new Venda();
        venda.AdicionarItem(new VendaItem { Preco = 50.0m, Quantidade = 2 });
        venda.AdicionarItem(new VendaItem { Preco = 30.0m, Quantidade = 1 });

        // Act
        decimal totalCalculado = venda.Total;

        // Assert
        Assert.Equal(130.0m, totalCalculado);
    }

}
