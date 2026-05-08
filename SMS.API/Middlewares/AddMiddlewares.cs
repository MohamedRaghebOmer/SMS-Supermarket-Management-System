namespace SMS.API.Middlewares
{
    public static class AddMiddlewares
    {
        public static void AddCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<AuditLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}