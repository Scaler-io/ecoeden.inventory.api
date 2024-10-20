using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Contracts.Caching;
public interface ICacheService
{
    CacheServiceType CacheServiceType { get; }
    Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default);
    Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync<T>(string key, T data);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
