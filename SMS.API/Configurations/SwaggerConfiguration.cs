using Microsoft.OpenApi;
using SMS.Shared.Constants;

namespace SMS.API.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Constants.ApiVersion, new OpenApiInfo
                {
                    Title = Constants.ApiTitle,
                    Version = Constants.ApiVersion
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Bearer token"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>() // FIXED: Changed Array.Empty<string>() to new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
