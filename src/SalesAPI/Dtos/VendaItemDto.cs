public class CriarVendaDto
{
    public int ClienteId { get; set; }
    public int FilialId { get; set; } 
    public List<VendaItemDto> Itens { get; set; }
}

public class VendaItemDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
    
    // Propriedades adicionais para preço original e desconto
    public decimal PrecoOriginal { get; set; }
    public decimal Desconto { get; set; }
}
