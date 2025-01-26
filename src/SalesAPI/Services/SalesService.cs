using SalesAPI.Data;
using SalesAPI.Models;
using SalesAPI.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAPI.Services
{
    public class SalesService
    {
        private readonly SalesDbContext _dbContext;
        private readonly IMongoDatabase? _mongoDb;

        public SalesService(SalesDbContext dbContext, IMongoDatabase mongoDb)
        {
           _mongoDb = mongoDb ?? throw new ArgumentNullException(nameof(mongoDb));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateOrder(int productId, int quantityRequested)
        {
            // Obtém a coleção de produtos do MongoDB
            var productCollection = _mongoDb.GetCollection<Produto>("Products");
            var product = await productCollection
                .Find(x => x.ProdutoId == productId)  // Comparação direta entre int e int
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            // Verifica se o estoque é suficiente
            if (product.QuantidadeEstoque < quantityRequested)
            {
                throw new InsufficientStockException("Estoque insuficiente para o produto.");
            }

            // Criação do pedido (exemplo simplificado)
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = productId, Quantity = quantityRequested, Price = product.Preco }  // Converte ProductId para string
                },
                TotalAmount = product.Preco * quantityRequested
            };

            // Adiciona o pedido ao banco de dados
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<decimal> CalculateTotal(Order order)
        {
            // Calcula o total do pedido considerando os descontos
            decimal total = order.OrderItems.Sum(item => item.Quantity * item.Price);
            total -= order.Discount;

            return total;
        }
    }
}
