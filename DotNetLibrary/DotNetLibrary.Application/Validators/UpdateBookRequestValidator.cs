using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class UpdateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(ubr => ubr.Title)
            .OptionalNotEmptyString(ubr => ubr.Title, "Title",
                BookConfiguration.TitleMaxLength);
        RuleFor(ubr => ubr.Author)
            .OptionalNotEmptyString(ubr => ubr.Author, "Author",
                BookConfiguration.AuthorMaxLength);
        RuleFor(ubr => ubr.PublicationDate)
            .OptionalNotEmpty("Publication date")
            .LessThan(DateOnly.FromDateTime(
                DateTime.Today.AddYears(BookConfiguration.PublicationDateMaxFutureYears)))
            .WithMessage("Publication date must be at most " +
                         $"{BookConfiguration.PublicationDateMaxFutureYears} years in the future")
            .WhenNotNull();
        RuleFor(ubr => ubr.Publisher)
            .OptionalNotEmptyString(ubr => ubr.Publisher, "Publisher",
                BookConfiguration.PublisherMaxLength);
        RuleFor(ubr => ubr.CategoryNames)
            .OptionalNotEmpty("Category names")
            .Must(cn => cn!.All(c => !string.IsNullOrWhiteSpace(c)))
            .WithMessage("Book category names must not be empty")
            .Must(cn => cn!.Count
                is >= BookConfiguration.MinCategories
                and <= BookConfiguration.MaxCategories)
            .WithMessage("Book must have between " +
                         $"{BookConfiguration.MinCategories} and " +
                         $"{BookConfiguration.MaxCategories} categories")
            .WhenNotNull();
    }
}