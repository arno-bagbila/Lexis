using Domain.Entities;
using LexisApi.Models.Input.Users.Create;
using MediatR;

namespace LexisApi.Features.Users.Create;

public record CreateCommand(CreateUser CreateUser) : IRequest<User>;