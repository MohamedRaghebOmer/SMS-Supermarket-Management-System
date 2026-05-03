using SMS.API.Configurations;
using SMS.API.Extensions;

namespace SMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Enable Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddCorsPolicy();

            // Register application and infrastructure services
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            builder.Services.AddAuthenticationConfiguration(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Run Swagger UI
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseGlobalExceptionHandling();
            app.UseHttpsRedirection();
            app.UseCors("SMSApiCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
