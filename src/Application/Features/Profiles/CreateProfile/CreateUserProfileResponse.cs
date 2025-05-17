using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Profiles.CreateProfile;

[SwaggerSchemaFilter(typeof(CreateUserProfileResponseSchemaFilter))]
public record CreateUserProfileResponse;

internal class CreateUserProfileResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
        };
    }
}