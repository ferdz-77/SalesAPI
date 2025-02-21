using MongoDB.Driver;
using SalesAPI.Models;
using SalesAPI.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SalesAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IMongoCollection<Produto> _produtoCollection;

        public ProdutoRepository(IMongoDatabase mongoDatabase)
        {
            _produtoCollection = mongoDatabase.GetCollection<Produto>("Products");
        }

        public async Task<Produto> GetByIdAsync(int productId)
        {
            return await _produtoCollection
                .Find(p => p.ProdutoId == productId)
                .FirstOrDefaultAsync();
        }
    }
}
