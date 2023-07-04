using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Users;

namespace LexisApi.IntegrationTests.Features.Users;

public class ListUserTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ListUserTests(WebApplicationFactory<Program> factory)
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
    public async Task ListUser_WithExistingUsers_ShouldReturnOK()
    {
        //arrange
        var createUser = new CreateUser
        {
            FirstName = $"Firstname_{Guid.NewGuid()}",
            LastName = $"LastName_{Guid.NewGuid()}"
        };        
        var secondCreateUser = new CreateUser
        {
            FirstName = $"Firstname_{Guid.NewGuid()}",
            LastName = $"LastName_{Guid.NewGuid()}"
        };

        await _client.PostAsJsonAsync("api/users", createUser); ;
        await _client.PostAsJsonAsync("api/users", secondCreateUser); ;

        // Act
        var response = await _client.GetAsync("api/users");
        var users = await response.BodyAs<IEnumerable<User>>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Count().Should().BeGreaterThan(2);
    }
}