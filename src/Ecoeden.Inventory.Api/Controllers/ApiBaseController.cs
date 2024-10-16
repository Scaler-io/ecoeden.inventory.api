using Ecoeden.Inventory.Api.Extensions;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecoeden.Inventory.Api.Controllers;

[Route("api/v{version:apiVersion}")]
[ApiController]
public class ApiBaseController(ILogger logger, IIdentityService identityService) : ControllerBase
{
    protected ILogger Logger { get; set; } = logger;
    private readonly IIdentityService _identityService = identityService;

    protected RequestInformation RequestInformation => new()
    {
        CorrelationId = GetOrGenerateCorrelationId(),
        CurrentUser = CurrentUser
    };


    private UserDto CurrentUser => _identityService.PrepareUser();
    private string GetOrGenerateCorrelationId() => Request?.GetRequestHeaderOrDefault("CorrelationId", $"GEN-{Guid.NewGuid()}");

    protected IActionResult OkOrFailure<T>(Result<T> result)
    {
        if (result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
        if (result.IsSuccess && result.Data == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
        if (result.IsSuccess && result.Data != null) return Ok(result.Data);

        return result.ErrorCode switch
        {
            ErrorCodes.BadRequest => BadRequest(new ApiValidationResponse(result.ErrorMessage)),
            ErrorCodes.InternalServerError => InternalServerError(new ApiExceptionResponse(result.ErrorMessage)),
            ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
            ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
            ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
            ErrorCodes.NotAllowed => BadRequest(new ApiResponse(ErrorCodes.NotAllowed, result.ErrorMessage)),
            _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
        };
    }

    private static ObjectResult InternalServerError(ApiResponse response)
    {
        return new ObjectResult(response)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes =
            [
                "application/json"
            ]
        };
    }

    protected IActionResult ProcessValidationResult(ValidationResult validationResult)
    {
        var errors = validationResult.Errors;
        var validationError = new ApiValidationResponse()
        {
            Errors = []
        };

        validationError.Errors.AddRange(
         errors.Select(error => new FieldLevelError
         {
             Code = error.ErrorCode,
             Field = error.PropertyName,
             Message = error.ErrorMessage
         })
        );

        return new BadRequestObjectResult(validationError);
    }

    public static bool IsInvalidResult(ValidationResult validationResult)
    {
        return validationResult != null && !validationResult.IsValid;
    }
}
