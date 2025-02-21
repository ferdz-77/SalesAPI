using SalesAPI.Data;
using SalesAPI.Models;
using SalesAPI.Exceptions;
using SalesAPI.Repositories.Interfaces;

namespace SalesAPI.Services
{
    public class SalesService
    {
        private readonly SalesDbContext _dbContext;
        private readonly IProdutoRepository _produtoRepository;

        public SalesService(SalesDbContext dbContext, IProdutoRepository produtoRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
        }

        public async Task CreateOrder(int productId, int quantityRequested)
        {
            var product = await _produtoRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ProdutoNotFoundException(productId);
            }

            if (product.QuantidadeEstoque < quantityRequested)
            {
                throw new InsufficientStockException("Estoque insuficiente para o produto.");
            }

            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = productId, Quantity = quantityRequested, Price = product.Preco }
                },
                TotalAmount = product.Preco * quantityRequested
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        public decimal CalculateTotal(Venda venda)
        {
            if (venda == null)
            {
                throw new ArgumentNullException(nameof(venda));
            }

            return venda.VendaItems.Sum(item => item.Preco * item.Quantidade);
        }

        public void AdicionarItem(Venda venda, VendaItem item)
        {
            if (venda == null)
                throw new ArgumentNullException(nameof(venda));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            // Corrigindo a adição de itens corretamente
            venda.AdicionarItem(item);
        }

        public class ProdutoNotFoundException : Exception
        {
            public ProdutoNotFoundException(int productId)
                : base($"Produto com ID {productId} não foi encontrado.")
            {
            }
        }
    }
}
