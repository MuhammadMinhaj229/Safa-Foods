using Microsoft.Extensions.Caching.Distributed;
using SafaFoods.Core.Services;

namespace SafaFoods.Infrastructure.Services;

public sealed class IdempotencyService(IDistributedCache cache) : IIdempotencyService
{
    private static string GetKey(string key) => $"idempotency:{key}";

    public async Task<string?> GetResponseAsync(string key, CancellationToken ct = default)
    {
        return await cache.GetStringAsync(GetKey(key), ct);
    }

    public async Task SetResponseAsync(string key, string responseBody, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(24) // Default 24h
        };
        
        await cache.SetStringAsync(GetKey(key), responseBody, options, ct);
    }
}
