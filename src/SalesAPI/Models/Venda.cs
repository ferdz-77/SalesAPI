using System.ComponentModel.DataAnnotations;


namespace SalesAPI.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }  // Id da venda, chave primária
        public int ClienteId { get; set; }  // Chave estrangeira
        public Cliente? Cliente { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.UtcNow; // Data da venda
        public decimal Total { get; set; } // Total da venda (calculado com base nos itens)

        public bool Cancelada { get; set; }  // Nova propriedade para indicar se a venda foi cancelada

        public ICollection<VendaItem> VendaItems { get; set; }

         // Chave estrangeira para Filial
        public int FilialId { get; set; } 
        public Filial Filial { get; set; }

        public Venda() { }
        
    }
}
