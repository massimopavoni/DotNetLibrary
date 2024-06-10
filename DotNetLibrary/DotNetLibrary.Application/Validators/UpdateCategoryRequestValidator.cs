using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(ucr => ucr.Description)
            .OptionalNotEmptyString(ucr => ucr.Description, "Description",
                CategoryConfiguration.DescriptionMaxLength);
    }
}