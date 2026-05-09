using SMS.API.Interfaces;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.ApplicationLogs;
using SMS.Shared.Enums;
using System.Diagnostics;

namespace SMS.API.Middlewares
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditLogService _auditLogService;
        private readonly IAuditLogRequestBuilder _auditLogRequestBuilder;
        private readonly IApplicationLogService _applicationLogService;

        public AuditLoggingMiddleware(RequestDelegate next, 
            IAuditLogService auditLogService,
            IAuditLogRequestBuilder auditLogRequestBuilder, 
            IApplicationLogService applicationLogService)
        {
            _next = next;
            _auditLogService = auditLogService;
            _auditLogRequestBuilder = auditLogRequestBuilder;
            _applicationLogService = applicationLogService;
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
            int auditLogId = 0;

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

                auditLogId =  await _auditLogService.AddAuditLogAsync(
                    await _auditLogRequestBuilder.BuildAsync(
                        context, responseBody, (int)stopwatch.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                ApplicationLogRequestDto logRequest = new ApplicationLogRequestDto
                {
                     AuditLogId = auditLogId,
                     Message = "An error occurred while creating audit log.",
                     Exception = ex,
                     StackTrace = ex.StackTrace
                };

                await _applicationLogService.AddAsync(logRequest);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private bool ShouldCreateAuditLog(HttpContext context, AuditActionType actionType)
        {
            return context.User.IsInRole("Admin") || IsCritical(actionType);
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