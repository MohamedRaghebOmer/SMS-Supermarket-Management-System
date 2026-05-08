using SMS.API.Helpers;
using SMS.API.Interfaces;

namespace SMS.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLayerHelpers(this IServiceCollection services)
        {
            services.AddScoped<IAuditActionTypeResolver, AuditActionTypeResolver>();
            services.AddScoped<IAttemptedUsernameResolver, AttemptedUsernameResolver>();
            services.AddScoped<IAuditLogRequestBuilder, AuditLogRequestBuilder>();
            return services;
        }
    }
}
