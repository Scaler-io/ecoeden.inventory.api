using Asp.Versioning;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Application.Features.Suppliers.Query.GetSupplier;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Swagger;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger.Examples.Supplier;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Ecoeden.Inventory.Api.Controllers.v1;

[ApiVersion("1")]
public class SupplierController(IMediator _mediator, ILogger logger, IIdentityService identityService) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = _mediator;

    [HttpGet("supplier/{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetSupplier", Description = "Fetches supplier details by id")]
    // 200
    [ProducesResponseType(typeof(SupplierDto), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SupplierResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorResponseEaxample))]
    public async Task<IActionResult> GetSupplier([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetSupplierQuery(id, RequestInformation);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
