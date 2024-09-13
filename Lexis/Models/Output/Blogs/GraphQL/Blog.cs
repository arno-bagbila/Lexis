using LexisApi.Models.Output.Users;

namespace LexisApi.Models.Output.Blogs.GraphQL;

public class Blog
{
    public string Id { get; set; } = null!;

    public string? Text { get; set; } = null!;

    public DateTime CreatedOn { get; private set; }

    public DateTime PublishedOn { get; set; }

    public string Category { get; set; } = null!;

    public User User { get; set; }  
}