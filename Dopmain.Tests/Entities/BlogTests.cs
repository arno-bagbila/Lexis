using Domain.Entities;
using FluentAssertions;
using MongoDB.Bson;

namespace Domain.Tests.Entities;

public class BlogTests
{
    [Fact]
    public void Create_AuthorIdEmpty_ThrowLexisException()
    {
        //act
        Action action = () => Blog.Create(ObjectId.Empty, "text", DateTime.Now.AddHours(1));

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Fact]
    public void Create_ValidAuthorId_CreatesBlog()
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();

        //act
        var blog = Blog.Create(authorId, "text", DateTime.Now.AddHours(1));

        //assert
        blog.Id.Should().NotBe(ObjectId.Empty);
        blog.AuthorId.Should().Be(authorId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidText_ThrowLexisException(string text)
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();

        //act
        Action action = () => Blog.Create(authorId, text, DateTime.Now.AddHours(1));

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetCategory_InvalidValues_ThrowLexisException(string category)
    {
        //arrange
        var authorId = ObjectId.GenerateNewId();
        var blog = Blog.Create(authorId, "Text", DateTime.Now.AddHours(1));

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
        var blog = Blog.Create(authorId, "Text", DateTime.Now.AddHours(1));
        var category = "Category";

        //act
        blog.SetCategory(category);

        //assert
        blog.Category.Should().Be("Category");
    }

    [Fact]
    public void Create_PublishedOnNotGreaterThanDateTimeNow_ThrowLexisException()
    {
        //act
        Action action = () => Blog.Create(ObjectId.Empty, "text", DateTime.Now.AddHours(-1));

        //assert
        Assert.Throws<LexisException>(() => action());
    }
}