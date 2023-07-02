using AutoMapper;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Blog = LexisApi.Models.Output.Blogs.Blog;

namespace LexisApi.Features.Blogs.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, Blog>
{
    private readonly IMongoCollection<Domain.Entities.Blog> _blogs;
    private readonly IMongoCollection<User> _users;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IMongoClient client, IMapper mapper)
    {
        _mapper = mapper;
        var database = client.GetDatabase("Lexis");
        var blogCollection = database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog));
        var userCollection = database.GetCollection<User>(nameof(User));
        _blogs = blogCollection;
        _users = userCollection;

    }

    public async Task<Blog> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var definition = command.CreateBlog;

        var authorFilter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(definition.AuthorId));
        var author =  await _users.Find(authorFilter).FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Cannot find User to assign the blog");

        var blog = Domain.Entities.Blog.Create(author.Id, definition.Text);
        await _blogs.InsertOneAsync(blog, cancellationToken: cancellationToken);

        return _mapper.Map<Blog>(blog);
    }
}