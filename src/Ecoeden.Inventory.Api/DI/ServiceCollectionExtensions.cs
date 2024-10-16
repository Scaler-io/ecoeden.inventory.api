
using Ecoeden.Inventory.Api.Middlewares;
using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ecoeden.Inventory.Api.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var env = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
        var logger = Logging.GetLogger(configuration, env);

        services.AddSingleton(logger);

        services.AddControllers()
            .AddNewtonsoftJson(configuration =>
            {
                configuration.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                configuration.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                configuration.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        // setup api versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = Asp.Versioning.ApiVersion.Default;
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // handles api's default error validation model
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = HandleFrameworkValidationFailure();
        });

        // middleware registrations
        services.AddTransient<CorrelationHeaderEnricher>();
        services.AddTransient<RequestLoggerMiddleware>();
        services.AddTransient<GlobalExceptionMiddleware>();

        var identityGroupAccess = configuration
        .GetSection("IdentityGroupAccess")
            .Get<IdentityGroupAccessOption>();

        // custom interface and its implementations
        services.AddSingleton<IIdentityService, IdentityService>();

        return services;
    }

    private static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
    {
        return context =>
        {
            var errors = context.ModelState
                            .Where(m => m.Value.Errors.Count > 0)
                            .ToList();

            var validationError = new ApiValidationResponse
            {
                Errors = []
            };

            foreach (var error in errors)
            {
                var fieldLevelError = new FieldLevelError
                {
                    Code = ErrorCodes.BadRequest.ToString(),
                    Message = error.Value.Errors?.First().ErrorMessage,
                    Field  = error.Key
                };
                validationError.Errors.Add(fieldLevelError);    
            }

            return new BadRequestObjectResult(validationError);
        };
    }
}
