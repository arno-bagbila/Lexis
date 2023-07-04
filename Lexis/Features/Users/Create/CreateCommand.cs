using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Users;
using MediatR;

namespace LexisApi.Features.Users.Create;

public record CreateCommand(CreateUser CreateUser) : IRequest<User>;