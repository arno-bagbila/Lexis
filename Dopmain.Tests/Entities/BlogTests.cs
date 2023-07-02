using Domain.Entities;
using FluentAssertions;
using MongoDB.Bson;

namespace Domain.Tests.Entities;

public class BlogTests
{
    [Fact]
    public void Create_AuthorIdEmpty_ThrowException()
    {
        //act
        Action action = () => Blog.Create(ObjectId.Empty, "text");

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Fact]
    public void Create_ValidAuthorId_CreatesBlog()
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();

        //act
        var blog = Blog.Create(authorId, "text");

        //assert
        blog.Id.Should().NotBe(ObjectId.Empty);
        blog.AuthorId.Should().Be(authorId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidText_ThrowException(string text)
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();

        //act
        Action action = () => Blog.Create(authorId, text);

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetCategory_InvalidValues_ThrowException(string category)
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();
        var blog = Blog.Create(authorId, "Text");

        //act
        Action action = () => blog.SetCategory(category);

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Fact]
    public void SetCategory_WithValidValue_CategoryIsSet()
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();
        var blog = Blog.Create(authorId, "Text");
        var category = "Category";

        //act
        blog.SetCategory(category);

        //assert
        blog.Category.Should().Be("Category");
    }
}