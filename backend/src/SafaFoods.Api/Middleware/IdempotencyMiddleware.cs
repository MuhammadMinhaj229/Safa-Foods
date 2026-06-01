using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Middleware;

public sealed class IdempotencyMiddleware(RequestDelegate next)
{
    private const string IdempotencyHeader = "X-Idempotency-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        // Only apply to POST/PUT/PATCH (mutating requests)
        if (!HttpMethods.IsPost(context.Request.Method) && 
            !HttpMethods.IsPut(context.Request.Method) && 
            !HttpMethods.IsPatch(context.Request.Method))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(IdempotencyHeader, out var key) || string.IsNullOrEmpty(key))
        {
            await next(context);
            return;
        }

        var idempotencyService = context.RequestServices.GetRequiredService<IIdempotencyService>();
        var cachedResponse = await idempotencyService.GetResponseAsync(key!);

        if (cachedResponse != null)
        {
            // Replay the cached response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        // Intercept the response to cache it
        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await next(context);

        if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            await idempotencyService.SetResponseAsync(key!, responseBody);
        }

        await responseBodyStream.CopyToAsync(originalBodyStream);
    }
}
