namespace SafaFoods.Core.Services;

public interface IIdempotencyService
{
    /// <summary>
    /// Checks if a request with the given key exists. 
    /// Returns the cached response if found, otherwise returns null.
    /// </summary>
    Task<string?> GetResponseAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Caches the response for a given idempotency key.
    /// </summary>
    Task SetResponseAsync(string key, string responseBody, TimeSpan? expiry = null, CancellationToken ct = default);
}
