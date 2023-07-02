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
    /// <returns><see cref="ValidationResult"/></returns>
    private static ValidationResult CanCreate(ObjectId authorId)
    {
        return authorId == ObjectId.Empty ? new ValidationResult("AuthorId is not valid") : ValidationResult.Success!;
    }

    /// <summary>
    /// Create a Blog
    /// </summary>
    /// <param name="authorId">Author Id linked to the Blog</param>
    /// <param name="text">Blog text</param>
    /// <returns>a <see cref="Blog"/></returns>
    /// <exception cref="Exception">If author Id is empty</exception>
    public static Blog Create(ObjectId authorId, string text)
    {
        var validationResult = CanCreate(authorId);
        if (validationResult != ValidationResult.Success)
        {
            throw LexisException.Create(LexisException.InvalidDataCode, validationResult.ErrorMessage!);
        }

        return new Blog
        {
            AuthorId = authorId,
            Text = text
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