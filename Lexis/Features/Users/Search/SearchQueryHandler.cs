using Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LexisApi.Features.Users.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, IEnumerable<User>>
{
    private readonly IMongoCollection<User> _users;

    public SearchQueryHandler(IMongoClient client)
    {
        var database = client.GetDatabase("Lexis");
        var collection = database.GetCollection<User>(nameof(User));
        _users = collection;
    }

    /// <summary>
    /// Handle request to get users
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <param name="cancellationToken">a cancellation token</param>
    /// <returns>list of <see cref="User"/></returns>
    public async Task<IEnumerable<User>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
    {
        var filter = string.IsNullOrWhiteSpace(searchQuery.Id)
            ? Builders<User>.Filter.Empty
            : Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(searchQuery.Id));
        var users = await _users.Find(filter).ToListAsync(cancellationToken);

        return users;
    }
}