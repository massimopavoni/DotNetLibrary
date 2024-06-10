using System.Text.RegularExpressions;
using DotNetLibrary.Models.Configurations;
using FluentValidation;

namespace DotNetLibrary.Application.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> RequiredNotEmpty<T, TProperty>
        (this IRuleBuilder<T, TProperty> ruleBuilder, string requiredField) =>
        ruleBuilder.NotEmpty().WithMessage($"{requiredField} is required not empty nor default");

    public static IRuleBuilderOptions<T, string> RequiredNotEmptyString<T>
        (this IRuleBuilder<T, string> ruleBuilder, string requiredField, int maxLength = -1)
    {
        var newRuleBuilder = ruleBuilder.RequiredNotEmpty(requiredField);
        return maxLength == -1
            ? newRuleBuilder
            : newRuleBuilder.MaximumLength(maxLength)
                .WithMessage($"{requiredField} must be at most {maxLength} characters long");
    }

    public static IRuleBuilderOptions<T, TProperty?> OptionalNotEmpty<T, TProperty>
        (this IRuleBuilder<T, TProperty?> ruleBuilder, string optionalField) =>
        ruleBuilder.NotEmpty().WithMessage($"{optionalField} is required not empty nor default");

    public static IRuleBuilderOptions<T, string?> OptionalNotEmptyString<T>
        (this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> whenField, string optionalField, int maxLength) =>
        ruleBuilder.RequiredNotEmpty(optionalField)
            .MaximumLength(maxLength)
            .WithMessage($"{optionalField} must be at most {maxLength} characters long")
            .When(t => whenField(t) != null);

    public static IRuleBuilderOptions<T, TProperty?> WhenNotNull<T, TProperty>
        (this IRuleBuilderOptions<T, TProperty?> ruleBuilder) =>
        ruleBuilder.When(t => t != null);

    public static IRuleBuilderOptionsConditions<T, string?> WhenNotEmptyString<T>
        (this IRuleBuilderOptionsConditions<T, string?> ruleBuilder, Func<T, string?> whenField) =>
        ruleBuilder.When(t => !string.IsNullOrWhiteSpace(whenField(t)));

    public static IRuleBuilderOptionsConditions<T, string> MatchingRegex<T>
        (this IRuleBuilder<T, string> ruleBuilder, string regex, string validationMessage) =>
        ruleBuilder.Custom((value, context) =>
        {
            if (!new Regex(regex).IsMatch(value!.ToString()))
                context.AddFailure(validationMessage);
        });

    public static IRuleBuilderOptions<T, string> ValidEmailAddress<T>
        (this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.EmailAddress().WithMessage("Email address is not valid");

    public static IRuleBuilderOptionsConditions<T, string?> ValidPassword<T>
        (this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.MinimumLength(UserConfiguration.PasswordBounds.Min)
            .WithMessage($"Password must be at least {UserConfiguration.PasswordBounds.Min} characters long")
            .MaximumLength(UserConfiguration.PasswordBounds.Max)
            .WithMessage($"Password must be at most {UserConfiguration.PasswordBounds.Max} characters long")
            .MatchingRegex(UserConfiguration.PasswordRegex,
                "Password must contain at least one digit, " +
                "one lowercase letter, one uppercase letter, one special character, and no spaces");
}