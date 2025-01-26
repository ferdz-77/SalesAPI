namespace SalesAPI.Models
{
    public class OrderItem
    {

        public int Id { get; set; } // Defina a chave primária
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        
    }
}
