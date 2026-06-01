using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace SafaFoods.Infrastructure.Services;

/// <summary>
/// Thin wrapper around IDistributedCache that handles JSON serialization/deserialization
/// and provides a clean generic API. Falls back gracefully if Redis is unavailable.
/// </summary>
public sealed class CacheService(IDistributedCache cache)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
    {
        try
        {
            var bytes = await cache.GetAsync(key, ct);
            if (bytes is null) return null;
            return JsonSerializer.Deserialize<T>(bytes, JsonOptions);
        }
        catch
        {
            // Redis unavailable — degrade gracefully, serve from DB
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default) where T : class
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
            };
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
            await cache.SetAsync(key, bytes, options, ct);
        }
        catch
        {
            // Redis unavailable — swallow, next request will re-populate
        }
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        try
        {
            await cache.RemoveAsync(key, ct);
        }
        catch { }
    }

    public async Task RemoveByPatternPrefixAsync(IEnumerable<string> keys, CancellationToken ct = default)
    {
        foreach (var key in keys)
            await RemoveAsync(key, ct);
    }
}
