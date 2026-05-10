using SMS.API.Interfaces;
using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Services;
using System.Diagnostics;

namespace SMS.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditLogService _auditLogService;
        private readonly IAuditLogRequestBuilder _auditLogRequestBuilder;
        private readonly IApplicationLogService _applicationLogService;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            IAuditLogService logService,
            IAuditLogRequestBuilder auditLogRequestBuilder,
            IApplicationLogService applicationLogService)
        {
            _next = next;
            _auditLogService = logService;
            _auditLogRequestBuilder = auditLogRequestBuilder;
            _applicationLogService = applicationLogService;
        }

        private async Task WriteErrorResponseAsync(HttpContext context,
            int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = message
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
                stopwatch.Stop();
            }
            catch (Exception ex) when (
            ex is ValidationException ||
            ex is ArgumentException)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                await WriteErrorResponseAsync(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                string responseMessage = "Internal Server Error";

                if (!context.Response.HasStarted)
                {
                    await WriteErrorResponseAsync(
                        context,
                        StatusCodes.Status500InternalServerError,
                        responseMessage);
                }

                try
                {
                    int? auditLogId = await LogAuditLogAsync(
                        context, responseMessage,
                        (int)stopwatch.ElapsedMilliseconds);
                    await LogApplicationLogAsync(ex, auditLogId);

                    await _applicationLogService.AddAsync(new Contracts.Requests.ApplicationLogs.ApplicationLogRequestDto
                    {
                        Exception = ex,
                        AuditLogId = auditLogId,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    });
                }
                catch { } // Swallow any exceptions from logging to avoid affecting the response to the client
            }
        }

        private async Task<int?> LogAuditLogAsync(HttpContext context,
            string responseBody, int duration)
        {
            try
            {
                var auditLogRequest = await _auditLogRequestBuilder.BuildAsync(context, responseBody, duration);
                return await _auditLogService.AddAsync(auditLogRequest);
            }
            catch { }

            return null;
        }

        private async Task LogApplicationLogAsync(Exception ex, int? auditLogId)
        {
            // Log the exception to application logs table here...
        }
    }
}
