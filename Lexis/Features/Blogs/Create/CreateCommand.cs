using Domain.Entities;
using LexisApi.Models.Input.Blogs.Create;
using MediatR;

namespace LexisApi.Features.Blogs.Create;

public record CreateCommand(CreateBlog CreateBlog) : IRequest<Blog>;