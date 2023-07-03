using AutoMapper;

namespace LexisApi.Models.Output.Users;

public class BlogAuthor
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}

public class BlogAuthorMappingProfile : Profile
{
    public BlogAuthorMappingProfile()
    {
        CreateMap<Domain.Entities.User, BlogAuthor>()
            .ForMember(b => b.Id, cfg =>
                cfg.MapFrom(b => b.Id.ToString()));
    }
}