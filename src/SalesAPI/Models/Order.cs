namespace SalesAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Propriedade para armazenar o desconto
        public decimal Discount { get; set; } = 0; // Definido com valor padrão de 0

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
