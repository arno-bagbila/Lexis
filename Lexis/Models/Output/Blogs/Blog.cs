using AutoMapper;
using LexisApi.Models.Output.Users;

namespace LexisApi.Models.Output.Blogs;

public class Blog
{
    public string Id { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime CreatedOn { get; private set; }

    public DateTime PublishedOn { get; set; }

    public string Category { get; set; } = null!;

    public BlogAuthor Author { get; set; } = null!;
}

public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        CreateMap<Domain.Entities.Blog, Blog>()
            .ForMember(b => b.Id, cfg =>
                cfg.MapFrom(b => b.Id.ToString()));
    }
}