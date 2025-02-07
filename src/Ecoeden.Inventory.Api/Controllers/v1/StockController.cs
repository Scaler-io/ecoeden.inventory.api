using Asp.Versioning;
using Ecoeden.Inventory.Api.Filters;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Application.Features.Stocks.Command.UpsertStock;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Ecoeden.Swagger.Examples.Supplier;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Ecoeden.Inventory.Application.Features.Stocks.Query.ListStocks;
using Contracts.Events;
using Ecoeden.Inventory.Application.Features.Stocks.Command.DeleteStock;

namespace Ecoeden.Inventory.Api.Controllers.v1;

[ApiVersion("1")]
[Authorize]
public class StockController(ILogger logger, IIdentityService identityService, IMediator mediator) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllStock", Description = "Lists all product stock")]
    // 200
    [ProducesResponseType(typeof(List<StockDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierListResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> GetAllStocks()
    {
        Logger.Here().MethodEntered();
        var query = new ListStockQuery();
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }


    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "UpsertStock", Description = "Creates or updates product stock")]
    // 200
    [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierListResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> UpsertStock([FromBody] StockDto stock)
    {
        Logger.Here().MethodEntered();
        var command = new UpsertStockCommand(stock, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteStock([FromQuery] string productId, [FromQuery] string supplierId)
    {
        Logger.Here().MethodEntered();
        var command = new DeleteStockCommand(productId, supplierId, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

}
