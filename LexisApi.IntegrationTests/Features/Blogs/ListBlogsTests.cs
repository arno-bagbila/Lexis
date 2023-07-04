using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using MongoDB.Bson;
using System.Net;
using LexisApi.IntegrationTests.Extensions;
using System.Net.Http.Json;
using LexisApi.Models.Output.Users;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Input.Blogs.Create;
using LexisApi.Models.Output.Blogs;

namespace LexisApi.IntegrationTests.Features.Blogs;

public class ListBlogsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ListBlogsTests(WebApplicationFactory<Program> factory)
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
    public async Task ListBlogs_WithExistingBlogs_ShouldReturnOK()
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
            PublishedOn = DateTime.Now.AddDays(1),
            Text = "Test"
        };

        var createSecondBlog = new CreateBlog
        {
            AuthorId = user.Id,
            Category = "Media",
            PublishedOn = DateTime.Now.AddDays(1),
            Text = "Test"
        };

        await _client.PostAsJsonAsync("api/blogs", createBlog);
        await _client.PostAsJsonAsync("api/blogs", createSecondBlog);

        // Act
        var response = await _client.GetAsync("api/blogs");
        var blogs = await response.BodyAs<IEnumerable<Blog>>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        blogs.Count().Should().BeGreaterThan(2);
    }
}