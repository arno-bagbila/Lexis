using Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LexisApi.Features.Blogs.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, IEnumerable<Blog>>
{
    private readonly IMongoCollection<Blog> _blogs;

    public SearchQueryHandler(IMongoClient client)
    {
        var database = client.GetDatabase("Lexis");
        var collection = database.GetCollection<Blog>(nameof(Blog));
        _blogs = collection;
    }

    public async Task<IEnumerable<Blog>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
    {
        var blogsQuery = _blogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery.Id))
        {
            blogsQuery =
                (MongoDB.Driver.Linq.IMongoQueryable<Blog>)blogsQuery.Where(x =>
                    x.Id == ObjectId.Parse(searchQuery.Id));
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.AuthorId))
        {
            blogsQuery = (MongoDB.Driver.Linq.IMongoQueryable<Blog>)blogsQuery.Where(x => x.AuthorId == ObjectId.Parse(searchQuery.AuthorId));
        }

        return await blogsQuery
            .ToListAsync(cancellationToken);
    }
}