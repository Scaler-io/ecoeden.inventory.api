using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples;
public sealed class InternalServerErrorResponseEaxample : IExamplesProvider<ApiExceptionResponse>
{
    public ApiExceptionResponse GetExamples()
    {
        return new(ErrorMessages.InternalServerError, new Exception().StackTrace);
    }
}
