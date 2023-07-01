using FluentValidation;

namespace LexisApi.Models.Input.Users.Create;

public class CreateUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithMessage("FirstName cannot be null or empty");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithMessage("LastName cannot be null or empty");
    }
}