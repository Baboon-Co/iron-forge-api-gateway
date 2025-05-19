using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Common.Validation.Rules;

public static partial class UserValidationRules
{
    [GeneratedRegex(@"^[a-zA-Z0-9_.\-]+$", RegexOptions.Compiled)]
    private static partial Regex LoginRegex();
    
    [GeneratedRegex(@"[!@#$%^&*(),.?""{}|<>]", RegexOptions.Compiled)]
    private static partial Regex SpecialSymbolRegex();
    
    [GeneratedRegex(@"[a-z]", RegexOptions.Compiled)]
    private static partial Regex LowercaseLetterRegex();
    
    [GeneratedRegex(@"[A-Z]", RegexOptions.Compiled)]
    private static partial Regex UppercaseLetterRegex();
    
    [GeneratedRegex(@"\d", RegexOptions.Compiled)]
    private static partial Regex DigitRegex();
    
    [GeneratedRegex(@"^[\p{L}\p{N}\-_\. ]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex NicknameRegex();
    
    public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Guid is required.")
            .Must(guid => Guid.TryParse(guid, out _)).WithMessage("Guid is not valid.");
    }
    
    public static IRuleBuilderOptions<T, string> Nickname<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(30).WithMessage("Username must be at most 30 characters long.")
            .Matches(NicknameRegex()).WithMessage("Nickname can only contain letters, numbers, underscores, dashes, periods and spaces.");
    }
    
    public static IRuleBuilderOptions<T, string> Login<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Login is required.")
            .MinimumLength(3).WithMessage("Login must be at least 3 characters long.")
            .MaximumLength(30).WithMessage("Login must be at most 30 characters long.")
            .Matches(LoginRegex()).WithMessage("Login can only contain letters, numbers, underscores, dashes and periods.");
    }

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Password must be at most 50 characters long.")
            .Matches(UppercaseLetterRegex()).WithMessage("Password must contain at least one uppercase letter.")
            .Matches(LowercaseLetterRegex()).WithMessage("Password must contain at least one lowercase letter.")
            .Matches(DigitRegex()).WithMessage("Password must contain at least one number.")
            .Matches(SpecialSymbolRegex()).WithMessage("Password must contain at least one special character.");
    }
}