using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Users;
using Microsoft.Extensions.Configuration;

namespace LexisApi.IntegrationTests.Features.Users;

public class CreateTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CreateTests(WebApplicationFactory<Program> factory)
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
    public async Task CreateUser_WithValidParameter_ShouldReturnCreated()
    {
        //arrange
        var firstName = $"Firstname_{Guid.NewGuid()}";
        var lastName = $"LastName_{Guid.NewGuid()}";
        var createUser = new CreateUser
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/users", createUser);
        var user = await response.BodyAs<User>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        user.Id.Should().NotBeNull();
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
    }
}