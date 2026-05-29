using Microsoft.AspNetCore.Authorization;
using SMS.API.Authorization;
using SMS.API.Authorization.Handlers;
using SMS.API.Helpers;
using SMS.API.Interfaces;
using SMS.Application.Interfaces.Services;

namespace SMS.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLayerHelpers(this IServiceCollection services)
        {
            services.AddScoped<IAuditActionTypeResolver, AuditActionTypeResolver>();
            services.AddScoped<IAttemptedUsernameResolver, AttemptedUsernameResolver>();
            services.AddScoped<IAuditLogRequestBuilder, AuditLogRequestBuilder>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IFileStoragePathProvider, WebRootFileStoragePathProvider>();

            return services;
        }
    }
}
