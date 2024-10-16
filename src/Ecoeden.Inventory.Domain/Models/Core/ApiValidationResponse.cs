using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class ApiValidationResponse : ApiResponse
{
    public ApiValidationResponse(string errorMessage = null) : base(ErrorCodes.BadRequest, errorMessage)
    {
        ErrorMessage = errorMessage ?? GetDefaultMessage(Code);
    }

    public List<FieldLevelError> Errors { get; set; }
    protected override string GetDefaultMessage(ErrorCodes code)
    {
        return "Invalid data provided";
    }
}
