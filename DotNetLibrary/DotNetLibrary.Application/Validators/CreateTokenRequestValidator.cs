using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateTokenRequestValidator : AbstractValidator<CreateTokenRequest>
{
    public CreateTokenRequestValidator()
    {
        RuleFor(r => r.EmailAddress)
            .RequiredNonEmpty("Email address")
            .ValidEmailAddress();
        RuleFor(r => r.Password)
            .RequiredNonEmpty("Password");
    }
}