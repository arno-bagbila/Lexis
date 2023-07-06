using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace LexisApi.IntegrationTests;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected static HttpClient Client = null!;
    private readonly WebApplicationFactory<Program> _factory;

    protected IntegrationTestBase()
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.json");

        _factory = new WebApplicationFactory<Program>();

        _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, conf) =>
            {
                conf.AddJsonFile(configPath);
            });

        });

        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
        Client.Dispose();
    }
}