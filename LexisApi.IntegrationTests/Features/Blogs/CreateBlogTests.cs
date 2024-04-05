using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Blogs.Create;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Blogs;
using LexisApi.Models.Output.Users;
using MongoDB.Bson;

namespace LexisApi.IntegrationTests.Features.Blogs;

public class CreateBlogTests : IntegrationTestBase
{

    [Fact]
    public async Task CreateBlog_WithNonExistingUser_ShouldReturnBadRequest()
    {
        //arrange
        var createBlog = new CreateBlog
        {
            AuthorId = ObjectId.GenerateNewId().ToString(),
            Category = "sport",
            PublishedOn = DateTime.Now.AddDays(1),
            Text = "Test"
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/blogs", createBlog);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateBlog_WithValidParameters_ShouldReturnCreated()
    {
        //arrange

        var createUser = new CreateUser
        {
            FirstName = $"Firstname_{Guid.NewGuid()}",
            LastName = $"LastName_{Guid.NewGuid()}"
        };

        var userResponse = await Client.PostAsJsonAsync("api/users", createUser);
        var user = await userResponse.BodyAs<User>();

        var createBlog = new CreateBlog
        {
            AuthorId = user.Id,
            Category = "sport",
            PublishedOn = DateTime.Now.AddDays(1),
            Text = "Test"
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/blogs", createBlog);
        var blog = await response.BodyAs<Blog>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        blog.Id.Should().NotBeNull();
        blog.Author.Should().NotBeNull();
        blog.Author.Id.Should().Be(user.Id);
        blog.Author.FirstName.Should().Be(user.FirstName);
        blog.Author.LastName.Should().Be(user.LastName);
        blog.Category.Should().Be("sport");
        blog.Text.Should().Be("Test");
    }

}