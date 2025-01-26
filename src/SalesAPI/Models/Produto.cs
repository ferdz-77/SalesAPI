using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(200)]
        public required string Nome { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser um valor positivo.")]
        public int QuantidadeEstoque { get; set; } // Definição da propriedade

        // Construtor com parâmetros
        public Produto(string nome, decimal preco, int quantidadeEstoque)
        {
            Nome = nome;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;  // Atribuindo o valor
        }

        // Construtor padrão (caso queira criar sem parâmetros)
        public Produto()
        {
            Nome = string.Empty;
            Preco = 0m;
            QuantidadeEstoque = 0;
        }

    }
}
