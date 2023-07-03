using AutoMapper;
using LexisApi.Models.Output.Blogs;
using FluentAssertions;
using MongoDB.Bson;
using Blog = Domain.Entities.Blog;

namespace LexisApi.Tests.Models.Output.Blogs;

public class BlogProfileTests
{
    private readonly IMapper _mapper;

    public BlogProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BlogMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MapAllFromBlogToOutputBlog()
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();
        var publishedOn = DateTime.Now.AddHours(1);
        var blog = Blog.Create(authorId, "Text", publishedOn);
        blog.SetCategory("Category");

        //act
        var result = _mapper.Map<LexisApi.Models.Output.Blogs.Blog>(blog);

        //assert
        result.Id.Should().Be(blog.Id.ToString());
        result.AuthorId.Should().Be(blog.AuthorId.ToString());
        result.Text.Should().Be(blog.Text);
        result.CreatedOn.Should().Be(blog.CreatedOn);
        result.PublishedOn.Should().Be(publishedOn);
        result.Category.Should().Be("Category");
    }
}