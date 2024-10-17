using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples;
public sealed class NotFoundResponseExample : IExamplesProvider<ApiResponse>
{
    public ApiResponse GetExamples()
    {
        return new(ErrorCodes.NotFound, ErrorMessages.NotFound);
    }
}
