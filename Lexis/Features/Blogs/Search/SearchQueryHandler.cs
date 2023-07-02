using AutoMapper;
using LexisApi.Models.Output.Blogs;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LexisApi.Features.Blogs.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, IEnumerable<Blog>>
{
    private readonly IMongoCollection<Domain.Entities.Blog> _blogs;
    private readonly IMapper _mapper;

    public SearchQueryHandler(IMongoClient client, IMapper mapper)
    {
        _mapper = mapper;
        var database = client.GetDatabase("Lexis");
        var collection = database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog));
        _blogs = collection;
    }

    public async Task<IEnumerable<Blog>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
    {
        var blogsQuery = _blogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery.Id))
        {
            blogsQuery =
                (MongoDB.Driver.Linq.IMongoQueryable<Domain.Entities.Blog>)blogsQuery
                    .Where(x => x.Id == ObjectId.Parse(searchQuery.Id));
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.AuthorId))
        {
            blogsQuery = (MongoDB.Driver.Linq.IMongoQueryable<Domain.Entities.Blog>)blogsQuery
                .Where(x => x.AuthorId == ObjectId.Parse(searchQuery.AuthorId));
        }

        var blogs =  await blogsQuery
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Blog>>(blogs);
    }
}