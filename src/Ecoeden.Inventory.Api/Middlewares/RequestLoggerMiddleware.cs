﻿
using Ecoeden.Inventory.Application.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System.Diagnostics;

namespace Ecoeden.Inventory.Api.Middlewares;

public class RequestLoggerMiddleware(ILogger logger) : IMiddleware
{
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value);
        var url = context.Request.GetDisplayUrl();
        var verb = context.Request.Method;

        using(LogContext.PushProperty("Url", url))
        {
            _logger.Here().Information("Http request starting");
            LogContext.PushProperty("HttpMethod", verb);

            await next(context);

            stopwatch.Stop();
            _logger.Here().Debug("Elapsed time {elapsedTime}", stopwatch.Elapsed);
            _logger.Here().Information("Http request completed. Response code {@code}", context.Response.StatusCode);
        }

    }
}
