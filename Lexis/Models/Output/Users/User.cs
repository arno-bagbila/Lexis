﻿using AutoMapper;

namespace LexisApi.Models.Output.Users;

public class User
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int TotalWordsCount { get; set; }

    public int PublishedBlogsCount { get; set; }
}

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Domain.Entities.User, User>()
            .ForMember(b => b.Id, cfg =>
                cfg.MapFrom(b => b.Id.ToString()));
    }
}