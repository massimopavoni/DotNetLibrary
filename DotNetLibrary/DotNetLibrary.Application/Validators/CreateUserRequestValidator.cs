using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.EmailAddress)
            .RequiredNonEmpty("Email address")
            .ValidEmailAddress();
        RuleFor(r => r.Password)
            .RequiredNonEmpty("Password")
            .ValidPassword();
    }
}