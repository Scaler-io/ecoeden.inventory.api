using Ecoeden.Inventory.Api;
using Ecoeden.Inventory.Api.DI;
using Ecoeden.Inventory.Api.Extensions;
using Ecoeden.Inventory.Application.DI;
using Ecoeden.Inventory.Infrastructure.Database;
using Ecoeden.Inventory.Infrastructure.DI;
using Ecoeden.Swagger;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var apiName = SwaggerConfiguration.ExtractApiNameFromEnvironmentVariable();
var apiDescription = builder.Configuration["ApiDescription"];
var apiHost = builder.Configuration["ApiOriginHost"];
var swaggerConfiguration = new SwaggerConfiguration(apiName, apiDescription, apiHost, builder.Environment.IsDevelopment());

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplicationServices(builder.Configuration, swaggerConfiguration)
    .AddBusinessLayerServices()
    .AddInfrastructureServices(configuration: builder.Configuration);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);


var app = builder.Build();

app.AddApplicationPipelines();

app.SeedMongoDatabase(async context =>
{
    await InventorySeeder.SeedAsync(context);
});

try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
