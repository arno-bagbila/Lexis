using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using LexisApi.Models.Output.Blogs;

namespace LexisApi.Tests.Models.Output.Users;

public class UserProfileTests
{
    private readonly IMapper _mapper;

    public UserProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(BlogMappingProfile)));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MapAllFromUserToOutputUser()
    {
        //arrange
        var user = User.Create("firstName", "lastName");

        //act
        var result = _mapper.Map<LexisApi.Models.Output.Users.User>(user);

        //assert
        result.Id.Should().Be(user.Id.ToString());
        result.FirstName.Should().Be(user.FirstName);
        result.LastName.Should().Be(user.LastName);
    }
}