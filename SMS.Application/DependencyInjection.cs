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
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductStockService, ProductStockService>();
            services.AddScoped<ICustomerLedgerService, CustomerLedgerService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IApplicationLogService, ApplicationLogService>();
            services.AddScoped<IRoleEntityPermissionService, RoleEntityPermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IDirectoryPathService, DirectoryPathService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ISaleItemService, SaleItemService>();

            return services;
        }
    }
}
