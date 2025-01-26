using Bogus;
using SalesAPI.Models;
using SalesAPI.Data;

public static class DatabaseSeeder
{
    public static void SeedProdutos(SalesDbContext context)
    {
        if (!context.Produtos.Any()) // Gera dados apenas se a tabela estiver vazia
        {
            var produtoFaker = new Faker<Produto>()
                .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
                .RuleFor(p => p.Preco, f => decimal.Parse(f.Commerce.Price(1, 100)))
                .RuleFor(p => p.QuantidadeEstoque, f => f.Random.Int(0, 500));

            var produtos = produtoFaker.Generate(50);
            context.Produtos.AddRange(produtos);
            context.SaveChanges();
            Console.WriteLine("50 produtos fictícios gerados com sucesso!");
        }
    }
    public static void SeedClientes(SalesDbContext context)
    {
        // Verificar se já existe algum cliente na base
        if (!context.Clientes.Any())
        {
            // Criar um cliente fictício
            var cliente = new Cliente
            {
                Nome = "João da Silva",
                Email = "joao.silva@email.com",
                CPF = "12345678901",  // CPF fictício
                DataCadastro = DateTime.UtcNow
            };

            // Adicionar o cliente ao contexto
            context.Clientes.Add(cliente);
            context.SaveChanges();  // Salvar na base de dados
            Console.WriteLine("Cliente teste cadastrado com sucesso!");
        }
    }
}
