using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Domain.Entities;

public class Blog
{
    private Blog()
    {
        CreatedOn = DateTime.Now;
        PublishedOn = DateTime.Now;
        Id = ObjectId.GenerateNewId();
    }

    private string _text = null!;

    /// <summary>
    /// Blog Id
    /// </summary>
    public ObjectId Id { get; private set; }

    /// <summary>
    /// Author Id
    /// </summary>
    public ObjectId AuthorId { get; private set; }

    /// <summary>
    /// Date when blog was published
    /// </summary>
    public DateTime PublishedOn { get; set; }

    public string Category { get; private set; } = null!;

    public string Text
    {
        get => _text;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw LexisException.Create(LexisException.InvalidDataCode, $"{nameof(Text)} cannot be null or empty");
            }

            _text = value;
        }
    }

    /// <summary>
    /// Date the blog was created
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Check if Blog can be created
    /// </summary>
    /// <param name="authorId">Author Id linked to the Blog</param>
    /// <param name="publishedOn">Date when the blog should be published</param>
    /// <returns><see cref="ValidationResult"/></returns>
    private static ValidationResult CanCreate(ObjectId authorId, DateTime publishedOn)
    {
        var errors = new List<(string Name, string Msg)>();

        if (authorId == ObjectId.Empty)
        {
            errors.Add((nameof(AuthorId), $"{nameof(AuthorId)} is not valid."));
        }

        if (publishedOn <= DateTime.Now)
        {
            errors.Add((nameof(PublishedOn), $"{nameof(PublishedOn)} should be greater than {DateTime.Now}"));
        }

        return (errors.Count > 0
            ? new ValidationResult(string.Join(Environment.NewLine, errors.Select(e => e.Msg)),
                errors.Select(e => e.Name).Distinct())
            : ValidationResult.Success)!;
    }

    /// <summary>
    /// Create a Blog
    /// </summary>
    /// <param name="authorId">Author Id linked to the Blog</param>
    /// <param name="text">Blog text</param>
    /// <param name="publishedOn">Date when the blog should be published</param>
    /// <returns>a <see cref="Blog"/></returns>
    /// <exception cref="Exception">If author Id is empty</exception>
    public static Blog Create(ObjectId authorId, string text, DateTime publishedOn)
    {
        var validationResult = CanCreate(authorId, publishedOn);
        if (validationResult != ValidationResult.Success)
        {
            throw LexisException.Create(LexisException.InvalidDataCode, validationResult.ErrorMessage!);
        }

        return new Blog
        {
            AuthorId = authorId,
            Text = text,
            PublishedOn = publishedOn
        };
    }

    /// <summary>
    /// Set Blog Categories
    /// </summary>
    /// <param name="category">Category to assign to the blogs</param>
    public void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw LexisException.Create(LexisException.InvalidDataCode, "Category cannot be null or empty");
        }

        Category = category;
    }
}