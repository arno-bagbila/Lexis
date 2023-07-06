using MongoDB.Bson;
using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Blogs.Create;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Blogs;
using LexisApi.Models.Output.Users;

namespace LexisApi.IntegrationTests.Features.Blogs;

public class GetBlogTests : IntegrationTestBase
{
    [Fact]
    public async Task GetBlog_WithWrongId_ShouldReturnNotFound()
    {
        // Act
        var response = await Client.GetAsync($"api/blogs/{ObjectId.GenerateNewId()}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetBlog_WithValidId_ShouldReturnOK()
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

        var blogResponse = await Client.PostAsJsonAsync("api/blogs", createBlog);
        var blog = await blogResponse.BodyAs<Blog>();

        // Act
        var response = await Client.GetAsync($"api/blogs/{blog.Id}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}