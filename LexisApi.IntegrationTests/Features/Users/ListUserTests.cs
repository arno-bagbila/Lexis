using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using LexisApi.IntegrationTests.Extensions;
using LexisApi.Models.Input.Users.Create;
using LexisApi.Models.Output.Users;

namespace LexisApi.IntegrationTests.Features.Users;

public class ListUserTests : IntegrationTestBase
{
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

        await Client.PostAsJsonAsync("api/users", createUser); ;
        await Client.PostAsJsonAsync("api/users", secondCreateUser); ;

        // Act
        var response = await Client.GetAsync("api/users");
        var users = await response.BodyAs<IEnumerable<User>>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Count().Should().BeGreaterThan(2);
    }
}