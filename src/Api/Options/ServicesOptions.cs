using FluentValidation;
using Utility.Configuration.Options.Abstractions;

namespace Api.Options;

public class ServicesOptions : IConfigurationOptions
{
    public static string SectionName => "Services";

    public string AuthServiceAddress { get; set; } = string.Empty;
}

public class ServicesOptionsValidator : AbstractValidator<ServicesOptions>
{
    public ServicesOptionsValidator()
    {
        RuleFor(o => o.AuthServiceAddress)
            .NotEmpty();
    }
}