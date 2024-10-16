using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class Result<T>
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCodes?ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Data = data, IsSuccess = true };
    }

    public static Result<T> Faliure(ErrorCodes errorCode, string errorMessage = null)
    {
        return new Result<T> { ErrorCode = errorCode, ErrorMessage = errorMessage, IsSuccess = false };
    }
}
