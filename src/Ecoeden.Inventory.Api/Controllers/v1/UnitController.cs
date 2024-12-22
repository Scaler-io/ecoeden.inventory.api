using Asp.Versioning;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Ecoeden.Swagger.Examples.Unit;
using Ecoeden.Inventory.Application.Features.Units.Query.ListUnit;
using MediatR;
using Ecoeden.Inventory.Application.Features.Units.Query.GetUnit;
using Ecoeden.Inventory.Application.Features.Units.Command.UpsertUnit;
using Ecoeden.Inventory.Application.Features.Units.Command.DeleteUnit;

namespace Ecoeden.Inventory.Api.Controllers.v1;

[Authorize]
[ApiVersion("1")]
public class UnitController(ILogger logger, IMediator mediator, IIdentityService identityService) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllUnits", Description = "Lists all units")]
    // 200
    [ProducesResponseType(typeof(List<UnitDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UnitListResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> GetAllUnits()
    {
        Logger.Here().MethodEntered();
        var query = new ListUnitQuery();
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetUnitDetails", Description = "Get unit details by id")]
    // 200
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UnitResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> GetUnitDetails([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetUnitQuery(id, RequestInformation);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "UpsertInit", Description = "Create or update unit")]
    // 200
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UnitResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> UpsertInit([FromBody] UnitDto dto)
    {
        Logger.Here().MethodEntered();
        var command = new UpsertUnitCommand(dto, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpDelete("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "DeleteUnit", Description = "Deletes unit")]
    // 200
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UnitDeleteResponseExmaple))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> DeleteUnit([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var command = new DeleteUnitCommand(id, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
