using SMS.Shared.Constants;
using System.Threading.RateLimiting;

namespace SMS.API.Configurations
{
    public static class RateLimitingConfiguration
    {
        public static IServiceCollection AddSlidingRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy(Constants.SlidingIp, httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 10, // max requests
                            Window = TimeSpan.FromSeconds(10), // total time window
                            SegmentsPerWindow = 5, // divides window into smaller segments (smooth limiting)
                            QueueLimit = 2,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
            });

            return services;
        }
    }
}
