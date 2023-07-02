using MediatR;

namespace LexisApi.Features.Users.Search;

public record SearchQuery(string? Id = null) : IRequest<IEnumerable<Models.Output.Users.User>>;
