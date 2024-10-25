using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ecoeden.Inventory.Infrastructure.Caching;
public sealed class DistributeCachingService(IDistributedCache distributedCache, IOptions<AppConfigOption> appConfigOptions) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;
    public CacheServiceType CacheServiceType { get; } = CacheServiceType.Distributed;

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        var data = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (data is null) return default;
        return JsonConvert.DeserializeObject<T>(data);
    }   

    public async Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default)
    {
        var serializedData = JsonConvert.SerializeObject(value);
        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(
                expirationTime ?? _appConfigOptions.CacheExpiration
            ));
        await _distributedCache.SetStringAsync(key, serializedData, cacheOptions, cancellation);
    }
    public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default)
    {
        return (await _distributedCache.GetStringAsync(key, cancellationToken)) is not null;
    }

    public async Task<T> UpdateAsync<T>(string key, T data)
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data));
        return await GetAsync<T>(key);
    }
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
