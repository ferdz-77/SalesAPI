using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Net;

public class VendaFunctionalTests : IClassFixture<SalesApiFactory>
{
    private readonly HttpClient _client;

    public VendaFunctionalTests(SalesApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CriarVenda_FluxoCompleto_DeveRetornar201EValidarBanco()
    {
        // Arrange: Criar um objeto de venda válido
        var novaVenda = new
        {
            ClienteId = 1,
            FilialId = 1,
            Itens = new[]
            {
                new { ProdutoId = 1, Quantidade = 2, Preco = 50.0m }
            }
        };

        // Act: Enviar requisição para a API
        var response = await _client.PostAsJsonAsync("/api/vendas", novaVenda);

        // Assert: Verificar se a venda foi criada corretamente
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // Pegar a URL da nova venda criada
        var location = response.Headers.Location?.ToString();
        Assert.NotNull(location);

        // Fazer uma requisição GET para validar se a venda existe no banco
        var getResponse = await _client.GetAsync(location);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }
}
