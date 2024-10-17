using Ecoeden.Inventory.Api.Middlewares;
using Ecoeden.Swagger;

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
                SwaggerConfiguration.SetupSwaggerUiOptions(option);
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
