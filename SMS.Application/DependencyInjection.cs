using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Helpers;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Services;
using SMS.Application.Services;

namespace SMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IValidationHelper, ValidationHelper>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IApplicationLogService, ApplicationLogService>();
            services.AddScoped<IRoleEntityPermissionService, RoleEntityPermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IDirectoryPathService, DirectoryPathService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
