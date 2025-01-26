namespace SalesAPI.Models
{
    public class VendaItem
    {

        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public decimal PrecoOriginal { get; set; }
        public int Quantidade { get; set; }


        // public int Id { get; set; } // Identificador único
         public int VendaId { get; set; } // Relacionamento com Venda
         public Venda Venda { get; set; }
        // public int ProdutoId { get; set; } // Relacionamento com Produto
        // public Produto Produto { get; set; }
        // public int Quantidade { get; set; } // Quantidade vendida
         public decimal Preco { get; set; }
        // public decimal PrecoOriginal { get; set; }  // Adicionando PrecoOriginal

        // public decimal PrecoUnitario { get; set; } // Preço unitário do produto
        // public decimal Subtotal { get; set; } // Subtotal (Quantidade * PreçoUnitário)
         public decimal Desconto { get; set; }  // Desconto aplicado no item
        // public decimal ValorTotalItem { get; set; }

        // Construtor sem parâmetros para o Entity Framework
        public VendaItem() { }

        // Construtor para inicializar o VendaItem, passando a Venda com o Cliente.
        public VendaItem(Produto produto, int quantidade, decimal preco, Venda venda)
        {
            Produto = produto ?? throw new ArgumentNullException(nameof(produto));
            Quantidade = quantidade;
            Preco = preco;
            Venda = venda ?? throw new ArgumentNullException(nameof(venda)); // Certificando que Venda não é nula

            if (venda.Cliente == null)
            {
                throw new ArgumentNullException(nameof(venda.Cliente), "Venda must have a Cliente.");
            }
        }
    }
}