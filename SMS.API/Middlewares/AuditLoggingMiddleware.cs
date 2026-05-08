using SMS.API.Interfaces;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.AuditLogs;
using SMS.Shared.Enums;
using System.Diagnostics;

namespace SMS.API.Middlewares
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditLogService _auditLogService;
        private readonly IAuditLogRequestBuilder _auditLogRequestBuilder;

        public AuditLoggingMiddleware(RequestDelegate next, IAuditLogService auditLogService, IAuditLogRequestBuilder auditLogRequestBuilder)
        {
            _next = next;
            _auditLogService = auditLogService;
            _auditLogRequestBuilder = auditLogRequestBuilder;
        }

        public async Task Invoke(HttpContext context, IAuditActionTypeResolver resolver, IAttemptedUsernameResolver attemptedUsernameResolver)
        {
            var actionType = resolver.Resolve(context);

            if (!ShouldCreateAuditLog(context, actionType))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                Stopwatch stopwatch = Stopwatch.StartNew();
                await _next(context);
                stopwatch.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var responseBody = await new StreamReader(context.Response.Body)
                    .ReadToEndAsync();

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBodyStream);

                context.Response.Body = originalBodyStream;

                await _auditLogService.AddAuditLogAsync(
                    await _auditLogRequestBuilder.BuildAsync(
                        context, responseBody, (int)stopwatch.ElapsedMilliseconds));
            }
            catch (Exception)
            {
                // Use application level logging here
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private bool ShouldCreateAuditLog(HttpContext context, AuditActionType actionType)
        {
            return context.User.IsInRole(nameof(Roles.Admin)) || IsCritical(actionType);
        }

        private static bool IsCritical(AuditActionType actionType)
        {
            return actionType == AuditActionType.Insert
                || actionType == AuditActionType.Update
                || actionType == AuditActionType.Delete
                || actionType == AuditActionType.Login
                || actionType == AuditActionType.Logout
                || actionType == AuditActionType.Register
                || actionType == AuditActionType.FailedLogin
                || actionType == AuditActionType.TokenRefresh
                || actionType == AuditActionType.TokenExpired
                || actionType == AuditActionType.AccessDenied;
        }
    }
}