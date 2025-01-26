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

        // Definnão das entidades
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<VendaItem> VendaItens { get; set; }

               
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Venda
            modelBuilder.Entity<Venda>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("nextval('\"Vendas_id_seq\"'::regclass)"); // Usar aspas duplas para a sequência

                // Relacionamentos
                entity.HasMany(v => v.VendaItems)
                    .WithOne(vi => vi.Venda)
                    .HasForeignKey(vi => vi.VendaId);

                entity.HasOne(v => v.Cliente)
                    .WithMany(c => c.Vendas)
                    .HasForeignKey(v => v.ClienteId);

                entity.ToTable("Vendas");
            });

            // Configuração de VendaItem
            modelBuilder.Entity<VendaItem>(entity =>
            {
                entity.HasKey(vi => vi.Id); // Define 'Id' como chave primária para VendaItem

                // Relacionamento com a entidade Produto
                entity.HasOne(vi => vi.Produto) // Relaciona com Produto
                    .WithMany() // Indica que Produto pode ter muitos VendaItems
                    .HasForeignKey(vi => vi.ProdutoId) // Define a chave estrangeira em VendaItem para Produto
                    .OnDelete(DeleteBehavior.Restrict); // Especifica que a exclusão de Produto não irá excluir os VendaItems
            });

            // Configuração de OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id); // Define 'Id' como chave primária para OrderItem
            });
        }
    }
}
