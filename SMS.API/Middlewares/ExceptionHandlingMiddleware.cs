using SMS.API.Interfaces;
using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Services;
using System.Diagnostics;

namespace SMS.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        public async Task InvokeAsync(HttpContext context,
            IAuditLogService auditLogService,
        IAuditLogRequestBuilder auditLogRequestBuilder,
        IApplicationLogService applicationLogService)
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
                    long? auditLogId = await LogAuditLogAsync(
                        context, 
                        auditLogRequestBuilder,
                        auditLogService, 
                        responseMessage,
                        (int)stopwatch.ElapsedMilliseconds);

                    await applicationLogService.AddAsync(new Contracts.Requests.ApplicationLogs.ApplicationLogRequestDto
                    {
                        Exception = ex,
                        AuditLogId = auditLogId,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    });
                }
                catch (Exception logEx)
                {
                    // If logging fails, there's not much we can do, but we can log to console as a last resort
                    Debug.WriteLine(logEx);
                }
            }
        }


        private async Task<long?> LogAuditLogAsync(HttpContext context,
            IAuditLogRequestBuilder auditLogRequestBuilder,
        IAuditLogService auditLogService,
            string responseBody, int duration)
        {
            var auditLogRequest = await auditLogRequestBuilder.BuildAsync(context, responseBody, duration);
            return await auditLogService.AddAsync(auditLogRequest);
        }
    }
}
