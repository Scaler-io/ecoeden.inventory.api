using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples;
public sealed class BadRequestResponseExample : IExamplesProvider<ApiValidationResponse>
{
    public ApiValidationResponse GetExamples()
    {
        return new(ErrorMessages.BadRequest);
    }
}
