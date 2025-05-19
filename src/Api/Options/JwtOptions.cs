using BaboonCo.Utility.Configuration.Options.Abstractions;
using FluentValidation;

namespace Api.Options;

public class JwtOptions : IConfigurationOptions
{
    public static string SectionName => "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

public class JwtOptionsValidator : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidator(IHostEnvironment environment)
    {
        RuleFor(o => o.Secret)
            .NotEqual("DEV_ONLY_KEY_DO_NOT_USE_IN_PRODUCTION").When(_ => environment.IsProduction())
            .Length(32, 64)
            .NotEmpty();

        RuleFor(o => o.Issuer)
            .NotEmpty();

        RuleFor(o => o.Audience)
            .NotEmpty();
    }
}