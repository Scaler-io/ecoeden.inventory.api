using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class ApiExceptionResponse : ApiResponse
{
    public ApiExceptionResponse(string errorMessage = null, string stacklTrace = null) 
        : base(ErrorCodes.InternalServerError, errorMessage)
    {
        StackTrace = stacklTrace;
    }

    public string StackTrace { get; set; }
}
