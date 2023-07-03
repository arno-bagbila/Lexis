using Domain.Entities;
using LexisApi.Infrastructure;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LexisApi.Features.Users.Search;

public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Models.Output.Users.User>>
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Blog> _blogs;

    public SearchQueryHandler(IMongoClient client) : base(client)
    {
        _users = Database.GetCollection<User>(nameof(User));
        _blogs = Database.GetCollection<Blog>(nameof(Blog));
    }

    /// <summary>
    /// Handle request to get users
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <param name="cancellationToken">a cancellation token</param>
    /// <returns>list of <see cref="User"/></returns>
    public async Task<IEnumerable<Models.Output.Users.User>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
    {
        var usersQuery = _users.AsQueryable();
        var blogsQuery = _blogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery.Id))
        {
            usersQuery =
                (MongoDB.Driver.Linq.IMongoQueryable<User>)usersQuery.Where(x => x.Id == ObjectId.Parse(searchQuery.Id));
        }

        var users = await usersQuery
            .ToListAsync(cancellationToken);

        var blogs = await blogsQuery
            .ToListAsync(cancellationToken);

        var userAndBlogs = users.GroupJoin(blogs, user => user.Id, blog => blog.Author.Id,
            (user, blogsList) => new { User = user, Blogs = blogsList }).ToList();


        return userAndBlogs.Select(x => new Models.Output.Users.User
        {
            FirstName = x.User.FirstName, 
            LastName = x.User.LastName, 
            Id = x.User.Id.ToString(),
            TotalWordsCount = string.Join(" ", x.Blogs.Select(blog => blog.Text)).Split(' ').ToList().Count,
            PublishedBlogsCount = x.Blogs.Count(blog => blog.PublishedOn < DateTime.Now)
        });
    }
}