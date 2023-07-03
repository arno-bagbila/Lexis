using AutoMapper;
using Domain;
using Domain.Entities;
using LexisApi.Infrastructure;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Blog = LexisApi.Models.Output.Blogs.Blog;

namespace LexisApi.Features.Blogs.Create;

public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, Blog>
{
    private readonly IMongoCollection<Domain.Entities.Blog> _blogs;
    private readonly IMongoCollection<User> _users;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IMongoClient client, IMapper mapper) : base(client)
    {
        _mapper = mapper;
        _blogs = Database.GetCollection<Domain.Entities.Blog>(nameof(Domain.Entities.Blog));
        _users = Database.GetCollection<User>(nameof(User));
    }

    public async Task<Blog> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var definition = command.CreateBlog;

        var authorFilter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(definition.AuthorId));
        var author =  await _users.Find(authorFilter).FirstOrDefaultAsync(cancellationToken) ?? 
                      throw LexisException.Create(LexisException.InvalidDataCode, "Cannot find User to assign the blog");

        var blog = Domain.Entities.Blog.Create(author, definition.Text, definition.PublishedOn);
        if (!string.IsNullOrWhiteSpace(definition.Category))
        {
            blog.SetCategory(definition.Category);
        }

        await _blogs.InsertOneAsync(blog, cancellationToken: cancellationToken);

        return _mapper.Map<Blog>(blog);
    }
}