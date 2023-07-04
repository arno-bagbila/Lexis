using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using MongoDB.Bson;
using FluentAssertions;
using LexisApi.Models.Input.Users.Create;
using System.Net.Http.Json;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Output.Users;
using LexisApi.Models.Input.Blogs.Create;

namespace LexisApi.IntegrationTests.Features.Users;

public class GetUserTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GetUserTests(WebApplicationFactory<Program> factory)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.json");

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, conf) =>
            {
                conf.AddJsonFile(configPath);
            });

        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUser_WithWrongId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync($"api/users/{ObjectId.GenerateNewId()}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUser_WithValidId_ShouldReturnOK()
    {
        //arrange
        var createUser = new CreateUser
        {
            FirstName = $"Firstname_{Guid.NewGuid()}",
            LastName = $"LastName_{Guid.NewGuid()}"
        };

        var userResponse = await _client.PostAsJsonAsync("api/users", createUser);
        var user = await userResponse.BodyAs<User>();

        // Act
        var response = await _client.GetAsync($"api/users/{user.Id}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUser_WithValidIdAndNoPublishedBlogs_ShouldReturnOK()
    {
        //arrange
        var createUser = new CreateUser
        {
            FirstName = $"Firstname_{Guid.NewGuid()}",
            LastName = $"LastName_{Guid.NewGuid()}"
        };

        var userResponse = await _client.PostAsJsonAsync("api/users", createUser);
        var user = await userResponse.BodyAs<User>();

        var createBlog = new CreateBlog
        {
            AuthorId = user.Id,
            Category = "sport",
            PublishedOn = new DateTime(2100, 12, 31),
            Text = "Test"
        };

        await _client.PostAsJsonAsync("api/blogs", createBlog);

        // Act
        var response = await _client.GetAsync($"api/users/{user.Id}");
        var blogAuthor = await response.BodyAs<User>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        blogAuthor.PublishedBlogsCount.Should().Be(0);
        blogAuthor.TotalWordsCount.Should().Be(1);
    }
}