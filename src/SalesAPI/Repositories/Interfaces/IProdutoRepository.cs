using SalesAPI.Models;
using System.Threading.Tasks;

namespace SalesAPI.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> GetByIdAsync(int productId);
    }
}
