using Ecoeden.Inventory.Api.Extensions;
using Serilog.Context;

namespace Ecoeden.Inventory.Api.Middlewares;

public sealed class CorrelationHeaderEnricher : IMiddleware
{
    private const string CorrelationIdLogPropertyName = "CorrelationId";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = GetOrGenerateCorrelationId(context);
        using (LogContext.PushProperty("ThreadId", Environment.CurrentManagedThreadId))
        {
            LogContext.PushProperty(CorrelationIdLogPropertyName, correlationId);
            context.Request.Headers.Append(CorrelationIdLogPropertyName, correlationId);
            await next(context);
        }
    }

    private static string GetOrGenerateCorrelationId(HttpContext context) => context?.Request
        .GetRequestHeaderOrDefault(CorrelationIdLogPropertyName, $"GEN-{Guid.NewGuid()}");
}
