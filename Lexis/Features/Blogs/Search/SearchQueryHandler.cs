using AutoMapper;
using IdentityModel;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Blog = LexisApi.Models.Output.Blogs.Blog;

namespace LexisApi.Features.Blogs.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, IEnumerable<Blog>>
{
    private readonly IMongoCollection<Domain.Entities.Blog> _blogs;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SearchQueryHandler(IMongoClient client, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        var database = client.GetDatabase("Lexis");
        var collection = database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog));
        _blogs = collection;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<Blog>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
    {
        var currentUserClaims = _httpContextAccessor.HttpContext!.User.Claims;

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

        //Setting the logic only if there is a claim in the context. If not we bypass this operation. 
        //As this is a simulation we don't want to altered the existing behavior.
        var userRole = currentUserClaims.FirstOrDefault(x => x.Value == JwtClaimTypes.Role);

        if (userRole == null || string.IsNullOrWhiteSpace(userRole.Value)) return _mapper.Map<IEnumerable<Blog>>(blogs);
        if (userRole.Value != "Sport")
        {
            blogs.RemoveAll(r => r.Category != "Sport");
        }

        return _mapper.Map<IEnumerable<Blog>>(blogs);
    }
}