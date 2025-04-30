using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Authentication.RefreshToken;

[SwaggerSchemaFilter(typeof(RefreshTokenResponseSchemaFilter))]
public record RefreshTokenResponse(
    string AccessToken, 
    string RefreshToken
);

internal class RefreshTokenResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["accessToken"] = new OpenApiString("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJJcm9uRm9yZ2UiLCJpYXQiOjE3NDU5MTQ5ODAsImV4cCI6MTc0NTkxNjE4MSwiYXVkIjoid3d3Lmlyb24tZm9yZ2UucnUiLCJzdWIiOiJhbHBhY2EifQ.TAJgeUTxhNN95rJCylojFclpOoYPmImpAUufKXvRnT8"),
            ["refreshToken"] = new OpenApiString("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJJcm9uRm9yZ2UiLCJpYXQiOjE3NDU5MTQ5ODAsImV4cCI6MTc3NzQ1MTAwNywiYXVkIjoid3d3Lmlyb24tZm9yZ2UucnUiLCJzdWIiOiJhbHBhY2EifQ.iwYqGzJquaWoMB1Mmm862vrjYYbHe6GUOLRcfE8GNCo"),
        };
    }
}