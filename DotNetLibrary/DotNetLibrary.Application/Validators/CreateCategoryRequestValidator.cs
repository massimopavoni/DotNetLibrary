using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(ccr => ccr.Name)
            .RequiredNotEmptyString("Name", CategoryConfiguration.NameMaxLength);
        RuleFor(ccr => ccr.Description)
            .OptionalNotEmptyString(ccr => ccr.Description, "Description",
                CategoryConfiguration.DescriptionMaxLength);
    }
}