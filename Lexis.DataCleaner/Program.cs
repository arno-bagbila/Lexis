using Lexis.DataCleaner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<CleanerService>();
        
        var configurationFile = Path.Combine("Configuration", "appsettings.json");
        var environmentConfigurationFile = Path.Combine("Configuration", $"appsettings.{builder.Environment.EnvironmentName}.json");
        
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationFile, optional: false, reloadOnChange: true)
            .AddJsonFile(environmentConfigurationFile, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        var connStr = builder.Configuration.GetConnectionString("MongoDb");
        builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connStr));
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Logging.AddSerilog(Log.Logger);
        
        var host = builder.Build();
        host.Run();
    }
}