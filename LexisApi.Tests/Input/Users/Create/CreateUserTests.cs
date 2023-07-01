using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using LexisApi.Models.Input.Blogs.Create;
using LexisApi.Models.Input.Users.Create;
using MongoDB.Bson;

namespace LexisApi.Tests.Input.Users.Create;

public class CreateUserTests
{
    private readonly CreateUserValidator _createUserValidator;

    public CreateUserTests()
    {
        _createUserValidator = new CreateUserValidator();
    }

    [Fact]
    public void CreateUser_InvalidFirstName_ShouldHaveError()
    {
        var invalidFirstNames = new List<string> { null!, string.Empty, " ", "" };

        foreach (var validator in invalidFirstNames
                     .Select(invalidFirstName => new CreateUser { FirstName = invalidFirstName })
                     .Select(model => _createUserValidator.TestValidate(model)))
        {
            validator.ShouldHaveValidationErrorFor(c => c.FirstName);
        }
    }

    [Fact]
    public void CreateUser_InvalidLastName_ShouldHaveError()
    {
        var invalidLastNames = new List<string> { null!, string.Empty, " ", "" };

        foreach (var validator in invalidLastNames
                     .Select(invalidLastName => new CreateUser { FirstName = invalidLastName })
                     .Select(model => _createUserValidator.TestValidate(model)))
        {
            validator.ShouldHaveValidationErrorFor(c => c.LastName);
        }
    }

    [Fact]
    public void CreateUser_ValidParameters_ShouldNotHaveError()
    {
        //arrange
        var firstName = "firstName";
        var lastName = "lastName";
        var model = new CreateUser { FirstName = firstName, LastName = lastName};

        //act
        var validator = _createUserValidator.TestValidate(model);

        //assert
        validator.ShouldNotHaveValidationErrorFor(c => c.FirstName);
        validator.ShouldNotHaveValidationErrorFor(c => c.LastName);
    }
}