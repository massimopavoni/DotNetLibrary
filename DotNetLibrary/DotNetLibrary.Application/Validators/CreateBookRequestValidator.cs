using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(cbr => cbr.ISBN)
            .RequiredNotEmptyString("ISBN", BookConfiguration.ISBNMaxLength);
        RuleFor(cbr => cbr.Title)
            .RequiredNotEmptyString("Title", BookConfiguration.TitleMaxLength);
        RuleFor(cbr => cbr.Author)
            .RequiredNotEmptyString("Author", BookConfiguration.AuthorMaxLength);
        RuleFor(cbr => cbr.PublicationDate)
            .RequiredNotEmpty("Publication date")
            .LessThan(DateOnly.FromDateTime(
                DateTime.Today.AddYears(BookConfiguration.PublicationDateMaxFutureYears)))
            .WithMessage("Publication date must be at most " +
                         $"{BookConfiguration.PublicationDateMaxFutureYears} years in the future");
        RuleFor(cbr => cbr.Publisher)
            .RequiredNotEmptyString("Publisher", BookConfiguration.PublisherMaxLength);
        RuleFor(cbr => cbr.CategoryNames)
            .RequiredNotEmpty("Category names")
            .Must(cn => cn.All(c => !string.IsNullOrWhiteSpace(c)))
            .WithMessage("Book category names must not be empty")
            .Must(cn => cn.Count
                is >= BookConfiguration.MinCategories
                and <= BookConfiguration.MaxCategories)
            .WithMessage("Book must have between " +
                         $"{BookConfiguration.MinCategories} and " +
                         $"{BookConfiguration.MaxCategories} categories");
    }
}