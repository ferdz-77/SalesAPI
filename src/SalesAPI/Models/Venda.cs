using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.UtcNow;

        public bool IsCanceled { get; set; }

        [Required]
        public int FilialId { get; set; }

        public Filial Filial { get; set; }

        private readonly List<VendaItem> _vendaItems = new();

        public IReadOnlyCollection<VendaItem> VendaItems => _vendaItems.AsReadOnly();

        public Venda() { }

        // Adicionando um método para incluir itens na venda
        public void AdicionarItem(VendaItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _vendaItems.Add(item);
        }
    }
}