using Application.Features.Authentication.Abstractions;
using Infrastructure.Features.Authentication;
using Infrastructure.Grpc;
using Infrastructure.Grpc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGrpcCallerService, GrpcCallerService>();

        return services;
    }
}