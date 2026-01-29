using System.Threading.RateLimiting;
using TeamChoice.WebApis.Domain.Configuration;

namespace TeamChoice.WebApis.Middlewares;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddConfiguredRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new RateLimitSettings();
        configuration.GetSection("RateLimitSettings").Bind(settings);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter =
                PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        ip,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = settings.PermitLimit,
                            Window = TimeSpan.FromMinutes(settings.WindowMinutes),
                            QueueLimit = settings.QueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            AutoReplenishment = true
                        });
                });
        });

        return services;
    }
}
