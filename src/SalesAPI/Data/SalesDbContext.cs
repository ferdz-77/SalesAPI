using Microsoft.EntityFrameworkCore;
using SalesAPI.Models; 

namespace SalesAPI.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        // Defina suas entidades aqui. Exemplo:
        public DbSet<Order> Orders { get; set; } // A entidade Order precisa ser reconhecida
    }
}
