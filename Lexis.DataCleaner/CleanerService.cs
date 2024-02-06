using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class CleanerService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IMongoClient _client;
    private readonly IConfiguration _configuration;
    
    public CleanerService(ILogger<CleanerService> logger, IMongoClient client, IConfiguration configuration)
    {
        _logger = logger;
        _client = client;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase(_configuration.GetValue<string>("ConnectionStrings:DatabaseName"));
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