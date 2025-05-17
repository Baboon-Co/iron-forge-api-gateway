using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Profiles.CreateProfile;

[SwaggerSchemaFilter(typeof(CreateUserProfileRequestSchemaFilter))]
public record CreateUserProfileRequest(string UserId, string Nickname);

internal class CreateUserProfileRequestSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["userId"] = new OpenApiString("a2f5d4d6-5a6d-4a3d-8a5d-6a4d5a3d4a5d"),
            ["nickname"] = new OpenApiString("Alpaca"),
        };
    }
}