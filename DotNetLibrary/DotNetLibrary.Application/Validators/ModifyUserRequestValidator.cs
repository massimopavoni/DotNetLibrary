using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.Requests;
using FluentValidation;

namespace DotNetLibrary.Application.Validators;

public class ModifyUserRequestValidator : AbstractValidator<ModifyUserRequest>
{
    public ModifyUserRequestValidator()
    {
        RuleFor(r => r.Password)
            .ValidPassword()
            .When(r => !string.IsNullOrWhiteSpace(r.Password), ApplyConditionTo.CurrentValidator);
    }
}