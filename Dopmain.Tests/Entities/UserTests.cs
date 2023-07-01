using Domain.Entities;
using MongoDB.Bson;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidFirstName_ThrowException(string firstName)
    {
        //act
        Action action = () => User.Create(firstName, "lastName");

        //assert
        Assert.Throws<Exception>(() => action());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidLastName_ThrowException(string lastName)
    {
        //act
        Action action = () => User.Create("firstName", lastName);

        //assert
        Assert.Throws<Exception>(() => action());
    }

    [Fact]
    public void Create_WithValidParameters_CreatesUser()
    {
        //arrange
        var firstName = "firstName";
        var lastName = "lastName";

        //act
        var user = User.Create(firstName, lastName);

        //assert
        user.Id.Should().NotBe(ObjectId.Empty);
        user.FirstName.Should().Be("firstName");
        user.LastName.Should().Be("lastName");
    }
}