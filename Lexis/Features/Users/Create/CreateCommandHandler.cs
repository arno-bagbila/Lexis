using AutoMapper;
using LexisApi.Infrastructure;
using LexisApi.Models.Output.Users;
using MediatR;
using MongoDB.Driver;

namespace LexisApi.Features.Users.Create;

public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, User>
{
    private readonly IMongoCollection<Domain.Entities.User> _users;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IMongoClient client, IConfiguration config, IMapper mapper) : base(client, config)
    {
        _users = Database.GetCollection<Domain.Entities.User>(nameof(Domain.Entities.User));
        _mapper = mapper;
    }

    /// <summary>
    /// Handle request to create a User
    /// </summary>
    /// <param name="command">command to create new user</param>
    /// <param name="cancellationToken">a cancellation token</param>
    /// <returns>created <see cref="User"/></returns>
    public async Task<User> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var definition = command.CreateUser;
        var user = Domain.Entities.User.Create(definition.FirstName, definition.LastName);
        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return _mapper.Map<User>(user);
    }
}