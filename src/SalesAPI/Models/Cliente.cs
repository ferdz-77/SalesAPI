using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public required string CPF { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public ICollection<Venda> Vendas { get; set; }

    }
}
