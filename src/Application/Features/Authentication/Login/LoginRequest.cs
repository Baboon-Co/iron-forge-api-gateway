using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Authentication.Login;

[SwaggerSchemaFilter(typeof(LoginRequestSchemaFilter))]
public record LoginRequest(
    string Login,
    string Password
);

internal class LoginRequestSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["login"] = new OpenApiString("alpaca"),
            ["password"] = new OpenApiString("cake"),
        };
    }
}