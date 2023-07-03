using AutoMapper;
using LexisApi.Models.Output.Blogs;
using FluentAssertions;
using Blog = Domain.Entities.Blog;
using User = Domain.Entities.User;

namespace LexisApi.Tests.Models.Output.Blogs;

public class BlogProfileTests
{
    private readonly IMapper _mapper;

    public BlogProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(BlogMappingProfile)));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MapAllFromBlogToOutputBlog()
    {
        //arrange
        var author = User.Create("firstName", "lastName");
        var publishedOn = DateTime.Now.AddHours(1);
        var blog = Blog.Create(author, "Text", publishedOn);
        blog.SetCategory("Category");

        //act
        var result = _mapper.Map<LexisApi.Models.Output.Blogs.Blog>(blog);

        //assert
        result.Id.Should().Be(blog.Id.ToString());
        result.Author.Id.Should().Be(blog.Author.Id.ToString());
        result.Author.FirstName.Should().Be(blog.Author.FirstName);
        result.Author.LastName.Should().Be(blog.Author.LastName);
        result.Text.Should().Be("Text");
        result.CreatedOn.Should().Be(blog.CreatedOn);
        result.PublishedOn.Should().Be(publishedOn);
        result.Category.Should().Be("Category");
    }
}