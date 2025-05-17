using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Features.Profiles.GetProfile;

[SwaggerSchemaFilter(typeof(GetUserProfileRequestSchemaFilter))]
public record GetUserProfileRequest(string Nickname);

internal class GetUserProfileRequestSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            {"nickname", new OpenApiString("Alpaca")}
        };
    }
}