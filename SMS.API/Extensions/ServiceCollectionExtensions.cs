namespace SMS.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            // e.g., services.AddScoped<IMyService, MyService>();
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register infrastructure services here
            // e.g., services.AddScoped<IMyRepository, MyRepository>();
            return services;
        }
    }
}