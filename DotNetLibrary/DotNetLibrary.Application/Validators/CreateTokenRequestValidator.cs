using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateTokenRequestValidator : AbstractValidator<CreateTokenRequest>
{
    public CreateTokenRequestValidator()
    {
        RuleFor(ctr => ctr.EmailAddress)
            .RequiredNotEmptyString("Email address", UserConfiguration.EmailAddressMaxLength)
            .ValidEmailAddress();
        RuleFor(ctr => ctr.Password)
            .RequiredNotEmptyString("Password");
    }
}