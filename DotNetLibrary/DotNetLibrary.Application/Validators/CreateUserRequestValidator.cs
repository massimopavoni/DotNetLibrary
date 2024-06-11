using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(cur => cur.EmailAddress)
            .RequiredNotEmptyString("Email address", UserConfiguration.EmailAddressMaxLength)
            .ValidEmailAddress();
        RuleFor(cur => cur.Password)
            .RequiredNotEmptyString("Password")
            .ValidPassword();
        RuleFor(cur => cur.FirstName)
            .OptionalNotEmptyString(cur => cur.FirstName, "First name",
                UserConfiguration.FirstNameMaxLength);
        RuleFor(cur => cur.LastName)
            .OptionalNotEmptyString(cur => cur.LastName, "Last name",
                UserConfiguration.LastNameMaxLength);
    }
}