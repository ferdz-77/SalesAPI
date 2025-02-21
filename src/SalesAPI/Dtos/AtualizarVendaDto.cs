using SalesAPI.Models;
using Xunit.Abstractions;

public class AtualizarVendaDto
{
    public int ClienteId { get; set; }
    public List<AtualizarVendaItemDto> Itens { get; set; }
}

public class AtualizarVendaItemDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }

    // Novas propriedades para armazenar o preço original e o desconto
    public decimal PrecoOriginal { get; set; }
    public decimal Desconto { get; set; }
}
