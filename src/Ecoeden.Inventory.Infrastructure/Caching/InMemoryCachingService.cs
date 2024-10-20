using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Infrastructure.Caching;
public sealed class InMemoryCachingService(IMemoryCache memoryCache, IOptions<AppConfigOption> appConfigOptions) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly AppConfigOption _appConfigOption = appConfigOptions.Value;
    public CacheServiceType CacheServiceType { get; } = CacheServiceType.InMemory;

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return _memoryCache.Get<T>(cacheKey);
    }
    public async Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        _memoryCache.Set<T>(key, value, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(45),
            AbsoluteExpirationRelativeToNow = expirationTime.HasValue
                ? TimeSpan.FromSeconds(expirationTime.Value)
                : TimeSpan.FromSeconds(_appConfigOption.CacheExpiration)
        });

    }
    public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return _memoryCache.TryGetValue(key, out _);
    }
    public async Task<T> UpdateAsync<T>(string key, T data)
    {
        await Task.CompletedTask;
        _memoryCache.Set<T>(key, data);
        return _memoryCache.Get<T>(key);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        _memoryCache.Remove(key);
    }

}
