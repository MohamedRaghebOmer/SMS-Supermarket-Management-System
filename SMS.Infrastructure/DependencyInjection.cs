using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Infrastructure.Data;
using SMS.Infrastructure.Helpers;
using SMS.Infrastructure.Repositories;

namespace SMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IDbHelper, DataAccessHelper>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            return services;
        }
    }
}
