using Domain.Entities;
using MediatR;

namespace LexisApi.Features.Blogs.Search;

public record SearchQuery(string? Id = null, string? AuthorId = null) : IRequest<IEnumerable<Blog>>;