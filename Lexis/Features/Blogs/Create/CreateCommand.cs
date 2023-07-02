using LexisApi.Models.Input.Blogs.Create;
using LexisApi.Models.Output.Blogs;
using MediatR;

namespace LexisApi.Features.Blogs.Create;

public record CreateCommand(CreateBlog CreateBlog) : IRequest<Blog>;