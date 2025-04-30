using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Authentication.Register;

[SwaggerSchemaFilter(typeof(RegisterRequestSchemaFilter))]
public record RegisterRequest(
    string Username,
    string Login,
    string Password
);

internal class RegisterRequestSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["username"] = new OpenApiString("Alpaca"),
            ["login"] = new OpenApiString("alpaca"),
            ["password"] = new OpenApiString("cake"),
        };
    }
}