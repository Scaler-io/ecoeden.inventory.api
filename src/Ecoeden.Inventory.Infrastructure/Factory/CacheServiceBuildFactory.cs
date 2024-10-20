using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Domain.Models.Enums;
using Ecoeden.Inventory.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Infrastructure.Factory;
public sealed class CacheServiceBuildFactory(IServiceProvider serviceProvider) : ICacheServiceBuildFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public ICacheService CreateService(CacheServiceType cacheServiceType)
    {
        return cacheServiceType switch
        {
            CacheServiceType.InMemory    => _serviceProvider.GetRequiredService<InMemoryCachingService>(),
            CacheServiceType.Distributed => _serviceProvider.GetRequiredService<DistributeCachingService>(),
            _                            => throw new ArgumentException("No such cache service has been defined")
        };
    }
}
