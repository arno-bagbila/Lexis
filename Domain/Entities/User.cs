using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    private User()
    {
        Id = ObjectId.GenerateNewId();
    }

    /// <summary>
    /// User Id
    /// </summary>
    public ObjectId Id { get; private set; }

    /// <summary>
    /// User first name
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// User last name
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Check if User can be created
    /// </summary>
    /// <param name="firstName">user first name</param>
    /// <param name="lastName">user last name</param>
    /// <returns><see cref="ValidationResult"/></returns>
    private static ValidationResult CanCreate(string firstName, string lastName)
    {
        var errors = new List<(string Name, string Msg)>();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add((nameof(FirstName), $"{nameof(FirstName)} cannot be null or empty"));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add((nameof(LastName), $"{nameof(LastName)} cannot be null or empty"));
        }

        return (errors.Count > 0
            ? new ValidationResult(string.Join(Environment.NewLine, errors.Select(e => e.Msg)),
                errors.Select(e => e.Name).Distinct())
            : ValidationResult.Success)!;
    }

    /// <summary>
    /// Create a User
    /// </summary>
    /// <param name="firstName">user firstname</param>
    /// <param name="lastName">user last name</param>
    /// <returns>a <see cref="User"/></returns>
    /// <exception cref="Exception">if firstname or lastname are null or empty</exception>
    public static User Create(string firstName, string lastName)
    {
        var validationResult = CanCreate(firstName, lastName);
        if (validationResult != ValidationResult.Success)
        {
            throw new Exception(validationResult.ErrorMessage);
        }

        return new User
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
}