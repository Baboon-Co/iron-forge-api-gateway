using Api.Common.Abstractions;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsWithFluentValidation<TOptions>(
        this IServiceCollection services
    ) where TOptions : class, IConfigurationOptions
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(TOptions.SectionName)
            .ValidateFluentValidation()
            .ValidateOnStart();

        return services;
    }
}