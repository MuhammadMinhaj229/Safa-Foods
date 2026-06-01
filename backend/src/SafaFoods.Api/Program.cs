using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

using SafaFoods.Api.Configuration;
using SafaFoods.Api.Endpoints;
using SafaFoods.Core.Extensions;
using SafaFoods.Core.Options;
using SafaFoods.Infrastructure.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddProblemDetails();
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks();
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.AddSafaFoodsCore();
    builder.Services.AddConfiguredOptions(builder.Configuration);
    builder.Services.AddSafaFoodsInfrastructure(builder.Configuration);

    // JWT Authentication
    var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
    var jwtSecret = jwtSection["Secret"] ?? throw new InvalidOperationException("JWT secret is not configured.");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSection["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CustomerPolicy", policy =>
            policy.RequireAuthenticatedUser()
                  .RequireClaim("customer_id"));
    });

    // Rate Limiting — Protect OTP endpoint from abuse
    builder.Services.AddRateLimiter(rateLimiter =>
    {
        // OTP Policy: max 3 requests per hour per IP
        rateLimiter.AddFixedWindowLimiter("otp_policy", opt =>
        {
            opt.PermitLimit = 3;
            opt.Window = TimeSpan.FromHours(1);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 0;
        });

        rateLimiter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    });

    var app = builder.Build();

    app.UseExceptionHandler();
    app.UseMiddleware<SafaFoods.Api.Middleware.CorrelationIdMiddleware>();
    app.UseMiddleware<SafaFoods.Api.Middleware.IdempotencyMiddleware>();
    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    var api = app.MapGroup("/api")
        .WithOpenApi();

    api.MapHealthEndpoints();
    api.MapCatalogEndpoints();
    api.MapDeliveryEndpoints();
    api.MapSubscriptionEndpoints();
    api.MapAddressEndpoints();
    api.MapOrderEndpoints();
    api.MapPaymentEndpoints();
    api.MapAdminAuthEndpoints();
    api.MapAdminEndpoints();
    api.MapMetaWebhookEndpoints();
    api.MapStoreEndpoints();
    api.MapCustomerAuthEndpoints();
    api.MapCartEndpoints();
    api.MapWalletEndpoints();
    api.MapCustomerProfileEndpoints();
    api.MapDeliverySlotEndpoints();

    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
