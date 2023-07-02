namespace LexisApi.Models.Output.Users;

public class User
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int TotalWordsCount { get; set; }
}