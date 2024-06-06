using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateTokenRequestValidator : AbstractValidator<CreateTokenRequest>
{
    public CreateTokenRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .RequiredNonEmpty("Email address")
            .ValidEmailAddress();
        RuleFor(x => x.Password)
            .RequiredNonEmpty("Password")
            .ValidPassword();
    }
}