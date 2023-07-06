using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Users;

namespace LexisApi.IntegrationTests.Features.Users;

public class CreateTests : IntegrationTestBase
{
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
        var response = await Client.PostAsJsonAsync("api/users", createUser);
        var user = await response.BodyAs<User>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        user.Id.Should().NotBeNull();
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
    }
}