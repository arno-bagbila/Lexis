using AutoMapper;
using FluentAssertions;
using LexisApi.Models.Output.Blogs;
using Domain.Entities;

namespace LexisApi.Tests.Models.Output.Users;

public  class BlogAuthorProfileTests
{
    private readonly IMapper _mapper;

    public BlogAuthorProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(BlogMappingProfile)));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MapAllFromUserToOutputBlogAuthor()
    {
        //arrange
        var author = User.Create("firstName", "lastName");

        //act
        var result = _mapper.Map<LexisApi.Models.Output.Users.BlogAuthor>(author);

        //assert
        result.Id.Should().Be(author.Id.ToString());
        result.FirstName.Should().Be(author.FirstName);
        result.LastName.Should().Be(author.LastName);
    }
}