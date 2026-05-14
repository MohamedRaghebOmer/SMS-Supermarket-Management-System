using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Interfaces.Services;
using SMS.Application.Services;

namespace SMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IApplicationLogService, ApplicationLogService>();
            services.AddScoped<IRoleEntityPermissionService, RoleEntityPermissionService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
