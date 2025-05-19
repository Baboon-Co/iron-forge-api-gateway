using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Authentication.Login;

[SwaggerSchemaFilter(typeof(LoginResponseSchemaFilter))]
public record LoginResponse(
    Guid UserId,
    string AccessToken,
    DateTime AccessTokenExpiration,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);

internal class LoginResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["userId"] = new OpenApiString("a2f5d4d6-5a6d-4a3d-8a5d-6a4d5a3d4a5d"),
            ["accessToken"] = new OpenApiString("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJJcm9uRm9yZ2UiLCJpYXQiOjE3NDU5MTQ5ODAsImV4cCI6MTc0NTkxNjE4MSwiYXVkIjoid3d3Lmlyb24tZm9yZ2UucnUiLCJzdWIiOiJhbHBhY2EifQ.TAJgeUTxhNN95rJCylojFclpOoYPmImpAUufKXvRnT8"),
            ["accessTokenExpiration"] = new OpenApiString("3023-01-01T00:00:00Z"),
            ["refreshToken"] = new OpenApiString("a2f5d4d6-5a6d-4a3d-8a5d-6a4d5a3d4a5d"),
            ["refreshTokenExpiration"] = new OpenApiString("3023-01-01T00:00:00Z"),
        };
    }
}