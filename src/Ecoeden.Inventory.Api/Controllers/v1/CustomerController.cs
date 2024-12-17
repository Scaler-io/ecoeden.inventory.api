using Asp.Versioning;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Application.Features.Customers.Query.ListCustomers;
using MediatR;
using Ecoeden.Inventory.Api.Filters;
using Ecoeden.Inventory.Domain.Models.Enums;
using Ecoeden.Inventory.Application.Features.Customers.Command.UpsertCustomer;
using Ecoeden.Inventory.Application.Features.Customers.Query.GetCustomer;
using Ecoeden.Inventory.Application.Features.Customers.Command.DeleteCustomer;
using Ecoeden.Swagger.Examples.Customer;

namespace Ecoeden.Inventory.Api.Controllers.v1;

[Authorize]
[ApiVersion("1")]
public class CustomerController(ILogger logger, IIdentityService identityService, IMediator mediator) 
    : ApiBaseController(logger, identityService)
{

    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "ListCustomers", Description = "Lists all customers")]
    // 200
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomerListResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> ListCustomers()
    {
        Logger.Here().MethodEntered();
        var query = new ListCustomerQuery();
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "ListCustomers", Description = "Lists all customers")]
    // 200
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomerDeleteResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> GetCustomerDetails([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetCustomerQuery(id, RequestInformation);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited(); 
        return OkOrFailure(result);
    }

    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "UpsertCustomer", Description = "Creates or updates customer")]
    // 200
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomerUpsertResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> UpsertCustomer([FromBody] CustomerDto customer)
    {
        Logger.Here().MethodEntered();
        var command = new UpsertCustomerCommand(customer, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpDelete("{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "DeleteCustomer", Description = "Deletes customer record")]
    // 200
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomerDeleteResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var command = new DeleteCustomerCommand(id, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
} 
