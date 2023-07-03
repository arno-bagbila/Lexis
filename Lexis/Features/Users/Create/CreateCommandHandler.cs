using Domain.Entities;
using LexisApi.Infrastructure;
using MediatR;
using MongoDB.Driver;

namespace LexisApi.Features.Users.Create;

public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, User>
{
    private readonly IMongoCollection<User> _users;

    public CreateCommandHandler(IMongoClient client) : base(client)
    {
        _users = Database.GetCollection<User>(nameof(User));
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
        var user = User.Create(definition.FirstName, definition.LastName);
        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }
}