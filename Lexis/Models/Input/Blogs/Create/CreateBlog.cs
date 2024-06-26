﻿using FluentValidation;
using MongoDB.Bson;

namespace LexisApi.Models.Input.Blogs.Create;

public class CreateBlog
{
    public string AuthorId { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string Category { get; set; } = null!;

    public DateTime PublishedOn { get; set; }
}

public class CreateBlogValidator : AbstractValidator<CreateBlog>
{
    public CreateBlogValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty()
            .WithMessage("Text cannot be null or empty");

        RuleFor(c => c.AuthorId)
            .NotEmpty()
            .WithMessage("AuthorId must be set")
            .Must(c => ObjectId.TryParse(c, out _))
            .WithMessage("AuthorId must be a valid ObjectId");

        RuleFor(c => c.PublishedOn)
            .GreaterThan(DateTime.Now);
    }
}