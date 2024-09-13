using GraphQL;
using GraphQL.Types;
using LexisApi.GraphQL.Types;
using LexisApi.Models.Output.Blogs.GraphQL;
using LexisApi.Models.Output.Users;
using MongoDB.Driver;

namespace LexisApi.GraphQL.Queries;

public sealed class LexisQuery : ObjectGraphType
{
    
    public LexisQuery(IMongoClient client, IConfiguration config)
    {
        var databaseName = config.GetValue<string>("ConnectionStrings:DatabaseName");
        var database = client.GetDatabase(databaseName);
        
        var blogs = database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog)).AsQueryable().ToList();
        var users = database.GetCollection<Domain.Entities.User>(nameof(Domain.Entities.User)).AsQueryable().ToList();
        var userAndBlogs = users.GroupJoin(blogs, user => user.Id, blog => blog.AuthorId,
            (user, blogsList) => new { User = user, Blogs = blogsList }).ToList();
        var usersToReturn =  userAndBlogs.Select(x => new User
        {
            FirstName = x.User.FirstName,
            LastName = x.User.LastName,
            Id = x.User.Id.ToString(),
            TotalWordsCount = x.Blogs.Any() ? string.Join(" ", x.Blogs.Select(blog => blog.Text!.Trim())).Split(' ').ToList().Count : 0,
            PublishedBlogsCount = x.Blogs.Count(blog => blog.PublishedOn < DateTime.Now)
        });

        var blogsToReturn = userAndBlogs.SelectMany(x => x.Blogs).Select(x => new Blog
        {
            Id = x.Id.ToString(),
            Text = x.Text!,
            PublishedOn = x.PublishedOn,
            Category = x.Category,
            User = usersToReturn.FirstOrDefault(user => user.Id == x.AuthorId.ToString())!
        });
        
        Field<ListGraphType<BlogType>>("blogs")
            .Description("Get all blogs")
            .Resolve(context => blogsToReturn);
        
        Field<BlogType>("blog")
            .Description("Get a blog by id")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "The unique identifier of the blog." }))
            .Resolve(context =>
            {
                var id = context.GetArgument<string>("id");
                var blog = blogsToReturn.FirstOrDefault(x => x.Id == id);
                return blog;
            });

        Field<ListGraphType<UserType>>("users")
            .Description("Get all users")
            .Resolve(context => usersToReturn);
        
        Field<UserType>("user").Description("Get a user by id")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "The unique identifier of the user." }))
            .Resolve(context =>
            {
                var id = context.GetArgument<string>("id");
                var user = usersToReturn.FirstOrDefault(x => x.Id == id);
                return user;
            });
    }
}