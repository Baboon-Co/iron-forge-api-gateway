using System.Text;
using Api.Extensions;
using Api.Infrastructure;
using Api.Options;
using FluentValidation;
using IronForge.Contracts.AuthService;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.Filters;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        AddAuth(services, configuration);
        AddMicroservices(services);
        AddServices(services);
        AddOpenApiDocs(services);

        return services;
    }

    private static void AddAuth(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithFluentValidation<JwtOptions>();

        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetRequired<JwtOptions>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });
        services.AddAuthorization();
    }

    private static void AddMicroservices(IServiceCollection services)
    {
        services.AddOptionsWithFluentValidation<ServicesOptions>();
        services.AddGrpcClient<Auth.AuthClient>((provider, options) =>
        {
            var servicesOptions = provider.GetRequiredService<ServicesOptions>();
            options.Address = new Uri(servicesOptions.AuthServiceAddress);
        });
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddFluentValidationAutoValidation();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddOpenApiDocs(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.EnableAnnotations();
            options.ExampleFilters();
        });
        services.AddSwaggerExamplesFromAssemblyOf<LoginRequest>();
    }
}