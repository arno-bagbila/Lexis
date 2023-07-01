using Domain.Entities;
using LexisApi.Features.Users.Search;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LexisApi.Features.Blogs.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, Blog>
{
    private readonly IMongoCollection<Blog> _blogs;
    private readonly IMongoCollection<User> _users;

    public CreateCommandHandler(IMongoClient client)
    {
        var database = client.GetDatabase("Lexis");
        var blogCollection = database.GetCollection<Blog>(nameof(Blog));
        var userCollection = database.GetCollection<User>(nameof(User));
        _blogs = blogCollection;
        _users = userCollection;

    }

    public async Task<Blog> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var definition = command.CreateBlog;
        var authorFilter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(definition.AuthorId));
        var author =  await _users.Find(authorFilter).FirstOrDefaultAsync(cancellationToken);

        if (author == null)
        {
            throw new Exception("Cannot find User to assign the blog");
        }

        var blog = Blog.Create(author.Id, definition.Text);
        await _blogs.InsertOneAsync(blog, cancellationToken: cancellationToken);
        return blog;
    }
}