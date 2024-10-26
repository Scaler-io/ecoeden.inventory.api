using Asp.Versioning;
using Ecoeden.Inventory.Api.Filters;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Application.Features.Suppliers.Command.DeleteSupplier;
using Ecoeden.Inventory.Application.Features.Suppliers.Command.UpsertSupplier;
using Ecoeden.Inventory.Application.Features.Suppliers.Query.GetSupplier;
using Ecoeden.Inventory.Application.Features.Suppliers.Query.ListSuppliers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Ecoeden.Swagger;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger.Examples.Supplier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Ecoeden.Inventory.Api.Controllers.v1;

[Authorize]
[ApiVersion("1")]
public class SupplierController(IMediator _mediator, ILogger logger, IIdentityService identityService) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = _mediator;

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "ListSuppliers", Description = "Lists all suppliers")]
    // 200
    [ProducesResponseType(typeof(List<SupplierDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierListResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> ListSuppliers()
    {
        Logger.Here().MethodEntered();
        var query = new ListSuppliersQuery(RequestInformation);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetSupplier", Description = "Fetches supplier details by id")]
    // 200
    [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> GetSupplier([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetSupplierQuery(id, RequestInformation);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateSupplier", Description = "Creates or updates supplier")]
    // 200
    [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> CreateOrUpdateSupplier([FromBody] SupplierDto supplier)
    {
        Logger.Here().MethodEntered();
        var command = new UpsertSupplierCommand(supplier, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpDelete("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "DeleteSupplier", Description = "Deletes supplier")]
    // 200
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierDeleteResponse))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> DeleteSupplier([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var command = new DeleteSupplierCommand(id, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
