using System.Text.RegularExpressions;
using FluentValidation;

namespace DotNetLibrary.Application.Extensions;

public static class ValidationExtensions
{
    private const string PasswordRegex = @"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* )$";
    private static readonly (int, int) PasswordBounds = (8, 256);

    public static IRuleBuilderOptions<T, TProperty> RequiredNonEmpty<T, TProperty>
        (this IRuleBuilder<T, TProperty> ruleBuilder, string requiredField) =>
        ruleBuilder.NotEmpty().WithMessage($"{requiredField} is required");

    public static void MatchingRegex<T, TProperty>
        (this IRuleBuilder<T, TProperty> ruleBuilder, string regex, string validationMessage) =>
        ruleBuilder.Custom((value, context) =>
        {
            if (!new Regex(regex).IsMatch(value!.ToString()!))
                context.AddFailure(validationMessage);
        });

    public static void ValidEmailAddress<T>
        (this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.EmailAddress().WithMessage("Email address is not valid");

    public static void ValidPassword<T>
        (this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.MinimumLength(PasswordBounds.Item1).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(PasswordBounds.Item2).WithMessage("Password must be at most 256 characters long")
            .MatchingRegex(PasswordRegex,
                "Password must contain at least one digit, " +
                "one lowercase letter, one uppercase letter, one special character, and no spaces");
}