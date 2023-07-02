using LexisApi.Models.Input.Blogs.Create;
using FluentValidation.TestHelper;
using MongoDB.Bson;

namespace LexisApi.Tests.Models.Input.Blogs.Create;

public class CreateBlogTests
{
    private readonly CreateBlogValidator _createBlogValidator;

    public CreateBlogTests()
    {
        _createBlogValidator = new CreateBlogValidator();
    }

    [Fact]
    public void CreateBlog_InvalidAuthorId_ShouldHaveError()
    {
        var invalidAuthorIds = new List<string> { null!, string.Empty, " ", "logoUrl", "" };

        foreach (var validator in invalidAuthorIds
                     .Select(invalidAuthorId => new CreateBlog { AuthorId = invalidAuthorId })
                     .Select(model => _createBlogValidator.TestValidate(model)))
        {
            validator.ShouldHaveValidationErrorFor(c => c.AuthorId);
        }
    }

    [Fact]
    public void CreateBlog_InvalidText_ShouldHaveError()
    {
        var invalidTexts = new List<string> { null!, string.Empty, " ", "" };

        foreach (var validator in invalidTexts
                     .Select(invalidText => new CreateBlog { Text = invalidText })
                     .Select(model => _createBlogValidator.TestValidate(model)))
        {
            validator.ShouldHaveValidationErrorFor(c => c.Text);
        }
    }

    [Fact]
    public void CreateBlog_ValidParameters_ShouldNotHaveError()
    {
        //arrange
        var authorId = ObjectId.GenerateNewId().ToString();
        var text = "BlogText";
        var model = new CreateBlog { AuthorId = authorId, Text = text, Category = "Category"};

        //act
        var validator = _createBlogValidator.TestValidate(model);

        //assert
        validator.ShouldNotHaveValidationErrorFor(c => c.AuthorId);
        validator.ShouldNotHaveValidationErrorFor(c => c.Text);
    }
}