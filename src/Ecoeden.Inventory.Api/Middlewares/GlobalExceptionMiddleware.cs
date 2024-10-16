﻿
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Enums;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;
using Ecoeden.Inventory.Application.Extensions;

namespace Ecoeden.Inventory.Api.Middlewares;

public sealed class GlobalExceptionMiddleware(ILogger logger, IWebHostEnvironment environment) : IMiddleware
{
    private readonly ILogger _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }catch (Exception ex)
        {
            await HandleGlobalException(context, ex);
        }
    }

    private async Task HandleGlobalException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = _environment.IsDevelopment()
                        ? new ApiExceptionResponse(ex.Message, ex.StackTrace)
                        : new ApiExceptionResponse(ex.Message);

        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            [
                new StringEnumConverter()
            ]
        };
        var jsonResponse = JsonConvert.SerializeObject(response, jsonSettings);
        _logger.Here().Error("{@InternalServerError} - {@response}", ErrorCodes.InternalServerError, jsonResponse);
        await context.Response.WriteAsync(jsonResponse);
    }
}
