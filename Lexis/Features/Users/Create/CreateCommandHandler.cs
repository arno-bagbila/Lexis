using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace LexisApi.Features.Users.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, User>
{
    private readonly IMongoCollection<User> _users;

    public CreateCommandHandler(IMongoClient client)
    {
        var database = client.GetDatabase("Lexis");
        var collection = database.GetCollection<User>(nameof(User));
        _users = collection;
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