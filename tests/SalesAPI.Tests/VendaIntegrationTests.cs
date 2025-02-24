using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;


public class VendaIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VendaIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    //[Fact]
    //public async Task CriarVenda_DeveRetornar201()
    //{
    //    // Arrange
    //    var novaVenda = new
    //    {
    //        ClienteId = 1,
    //        FilialId = 1,cls
    //        Itens = new[]
    //        {
    //            new { ProdutoId = 1, Quantidade = 2, Preco = 50.0m }
    //        }
    //    };

    //    // Act
    //    var response = await _client.PostAsJsonAsync("/api/vendas", novaVenda);

    //    // Assert
    //    response.EnsureSuccessStatusCode(); // Verifica se o status é 2xx
    //    Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    //}
}
