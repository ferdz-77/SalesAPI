using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using Microsoft.AspNetCore.Hosting;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover o contexto de banco de dados real
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SalesDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Adicionar um banco de dados em memória para testes
            services.AddDbContext<SalesDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Criar um escopo para aplicar a migração e preparar o banco de dados fake
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<SalesDbContext>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development"); // Garante que as configurações de dev sejam carregadas
    }
}
