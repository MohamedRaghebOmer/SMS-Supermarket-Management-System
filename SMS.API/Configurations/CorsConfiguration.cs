namespace SMS.API.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("SMSApiCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://localhost:7291",
                            "http://localhost:5092"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
