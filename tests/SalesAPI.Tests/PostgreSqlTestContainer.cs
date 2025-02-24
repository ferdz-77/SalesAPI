using Testcontainers.PostgreSql;
using System.Threading.Tasks;

public class PostgreSqlTestContainer : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; }

    public PostgreSqlTestContainer()
    {
        Container = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.StopAsync();
    }
}
