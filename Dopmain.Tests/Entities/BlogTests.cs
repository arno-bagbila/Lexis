using Domain.Entities;
using FluentAssertions;
using MongoDB.Bson;

namespace Domain.Tests.Entities;

public class BlogTests
{
    private readonly User _author;

    public BlogTests()
    {
        _author = User.Create("firstName", "lastName");
    }

    [Fact]
    public void Create_AuthorIsNull_ThrowLexisException()
    {
        //act
        Action action = () => Blog.Create(null!, "text", DateTime.Now.AddHours(1));

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Fact]
    public void Create_ValidParameters_CreatesBlog()
    {
        //arrange
        var publishedOn = DateTime.Now.AddHours(1);

        //act
        var blog = Blog.Create(_author, "text", publishedOn);

        //assert
        blog.Id.Should().NotBe(ObjectId.Empty);
        blog.Author.Should().Be(_author);
        blog.PublishedOn.Should().Be(publishedOn);
        blog.Text.Should().Be("text");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidText_ThrowLexisException(string text)
    {

        //act
        Action action = () => Blog.Create(_author, text, DateTime.Now.AddHours(1));

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
        var blog = Blog.Create(_author, "Text", DateTime.Now.AddHours(1));

        //act
        Action action = () => blog.SetCategory(category);

        //assert
        Assert.Throws<LexisException>(() => action());
    }

    [Fact]
    public void SetCategory_WithValidValue_CategoryIsSet()
    {
        //arrange
        var blog = Blog.Create(_author, "Text", DateTime.Now.AddHours(1));
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
        Action action = () => Blog.Create(_author, "text", DateTime.Now.AddHours(-1));

        //assert
        Assert.Throws<LexisException>(() => action());
    }
}