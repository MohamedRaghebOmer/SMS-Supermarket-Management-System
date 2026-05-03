using Microsoft.Extensions.DependencyInjection;

namespace SMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register infrastructure services here
            // For example:
            // services.AddScoped<IMyService, MyService>();
            return services;
        }
    }
}
