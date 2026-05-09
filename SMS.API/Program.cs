using SMS.API.Configurations;
using SMS.API.Middlewares;
using SMS.Application;
using SMS.Infrastructure;

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

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddApiLayerHelpers();

            builder.Services.AddAuthenticationConfiguration(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Run Swagger UI
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.AddCustomMiddlewares();
            app.UseHttpsRedirection();
            app.UseCors("SMSApiCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
