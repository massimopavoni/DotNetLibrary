using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(uur => uur.Password)
            .ValidPassword()
            .WhenNotEmptyString(uur => uur.Password);
        RuleFor(uur => uur.FirstName)
            .OptionalNotEmptyString(uur => uur.FirstName, "First name",
                UserConfiguration.FirstNameMaxLength);
        RuleFor(uur => uur.LastName)
            .OptionalNotEmptyString(uur => uur.LastName, "Last name",
                UserConfiguration.LastNameMaxLength);
    }
}