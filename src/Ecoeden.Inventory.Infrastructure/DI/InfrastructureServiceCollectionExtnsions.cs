using Ecoeden.Inventory.Application.Contracts.Database;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Application.Contracts.EventBus;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Contracts.Security;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Infrastructure.Caching;
using Ecoeden.Inventory.Infrastructure.Database;
using Ecoeden.Inventory.Infrastructure.Database.Repositories;
using Ecoeden.Inventory.Infrastructure.Database.SQL;
using Ecoeden.Inventory.Infrastructure.Database.SQL.Repositories;
using Ecoeden.Inventory.Infrastructure.EventBus;
using Ecoeden.Inventory.Infrastructure.Factory;
using Ecoeden.Inventory.Infrastructure.HealthCheck;
using Ecoeden.Inventory.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecoeden.Inventory.Infrastructure.DI;
public static class InfrastructureServiceCollectionExtnsions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck<DbHealthCheck>("mongodb-health", HealthStatus.Unhealthy);

        services.AddScoped(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoDbOption>>();
            return new MongoClient(mongoOptions.Value.ConnectionString);
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = configuration["Redis:InstanceName"];
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddMemoryCache();
        services.AddScoped<ICacheServiceBuildFactory, CacheServiceBuildFactory>();
        services.AddScoped<DistributeCachingService>();
        services.AddScoped <InMemoryCachingService>();
        services.AddScoped<IInventoryDbContext, InventoryDbContext>();
        services.AddScoped(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
        services.AddScoped<IPermissionMapper, PermissionMapper>();

        services.AddDbContext<EcoedenDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Sqlserver"));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddMassTransit(config =>
        {
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("suppliers", false));
            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMq = configuration.GetSection(EventBusOption.OptionName).Get<EventBusOption>();
                cfg.Host(rabbitMq.Host, "/", host =>
                {
                    host.Username(rabbitMq.Username);
                    host.Password(rabbitMq.Password);
                });
            });
        });

        services.AddScoped(typeof(IPublishService<,>), typeof(PublishService<,>));
        services.AddScoped<IPublishServiceFactory, PublishServiceFactory>();

        return services;
    }
}
