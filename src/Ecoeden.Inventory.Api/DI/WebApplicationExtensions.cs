using Asp.Versioning.ApiExplorer;
using Ecoeden.Inventory.Api.Middlewares;
using Ecoeden.Swagger;
using HealthChecks.UI.Client;

namespace Ecoeden.Inventory.Api.DI;

public static class WebApplicationExtensions
{
    public static WebApplication AddApplicationPipelines(this WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger(SwaggerConfiguration.SetupSwaggerOptions)
            .UseSwaggerUI(option =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                SwaggerConfiguration.SetupSwaggerUiOptions(option, provider);
            });

        app.MapHealthChecks("api/v1/health", new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseMiddleware<CorrelationHeaderEnricher>()
            .UseMiddleware<RequestLoggerMiddleware>()
            .UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
