using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ecoeden.Swagger;
public class SwaggerApiVersionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];
        operation.Parameters.Add(new()
        {
            Name = "Api-Version",
            In = ParameterLocation.Header,
            Schema = new() { Type = "string" },
            Description = "Vesrion of the api. Example v1",
            Required = true,
        });
    }
}
