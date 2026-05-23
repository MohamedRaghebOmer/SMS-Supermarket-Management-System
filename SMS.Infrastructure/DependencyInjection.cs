using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Helpers;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Infrastructure.Data;
using SMS.Infrastructure.Helpers;
using SMS.Infrastructure.Repositories;

namespace SMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IApplicationLogRepository, ApplicationLogRepository>();
            services.AddScoped<IRoleEntityPermissionRepository, RoleEntityPermissionsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IStringHelper, StringHelper>();
            services.AddScoped<IRolesRepository, RolesRepository>();

            return services;
        }
    }
}
