using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Lexis.DataCleaner;

public class CleanerService(ILogger<CleanerService> logger, IMongoClient client, IConfiguration configuration)
    : IHostedService
{
    private readonly ILogger _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var database = client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:DatabaseName"));
        var blogCollection = database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog));
        var filter = Builders<Domain.Entities.Blog>.Filter.Lt(x => x.PublishedOn, DateTime.Now.AddHours(-1));
        var result = blogCollection.DeleteMany(filter);
        _logger.LogInformation($"Deleted {result.DeletedCount} documents");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}