using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesAPI;
using SalesAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class SalesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover o contexto de banco de dados existente
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SalesDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adicionar um banco de dados em memória para os testes funcionais
            services.AddDbContext<SalesDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDbFunctional");
            });

            // Construir o provedor de serviços
            var provider = services.BuildServiceProvider();

            // Criar um escopo e inicializar o banco de dados
            using (var scope = provider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SalesDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
