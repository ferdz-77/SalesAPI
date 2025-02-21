using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

public class VendaItemDto
{
    public VendaItemDto(int produtoId, int quantidade, decimal preco, decimal precoOriginal, decimal desconto)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        Preco = preco;
        PrecoOriginal = precoOriginal;
        Desconto = desconto;
    }

    [Required(ErrorMessage = "O ProdutoId é obrigatório.")]
    public int ProdutoId { get; }

    [Required(ErrorMessage = "A Quantidade é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; }

    [Required(ErrorMessage = "O Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; }

    public decimal PrecoOriginal { get; }
    public decimal Desconto { get; }

    // Método que retorna um novo objeto com desconto aplicado
    public VendaItemDto ComDescontoAplicado()
    {
        var desconto = Quantidade >= 10 ? 0.20m : Quantidade >= 4 ? 0.10m : 0;
        var precoComDesconto = PrecoOriginal * (1 - desconto);

        return new VendaItemDto(ProdutoId, Quantidade, precoComDesconto, PrecoOriginal, desconto);
    }
}
