using Ecoeden.Inventory.Api.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.AddApplicationPipelines(app.Environment.IsDevelopment());

try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
