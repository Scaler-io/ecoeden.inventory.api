using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Contracts.Factory;
public interface ICacheServiceBuildFactory
{
    ICacheService CreateService(CacheServiceType cacheServiceType);
}
