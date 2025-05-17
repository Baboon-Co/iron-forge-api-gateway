using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Profiles.GetProfile;

[SwaggerSchemaFilter(typeof(GetUserProfileResponseSchemaFilter))]
public record GetUserProfileResponse(int Wins, int Losses);

internal class GetUserProfileResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["wins"] = new OpenApiInteger(10),
            ["losses"] = new OpenApiInteger(8),
        };
    }
}