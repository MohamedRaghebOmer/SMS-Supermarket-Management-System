using SMS.Contracts.Requests.ApplicationLogs;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public static class ApplicationLogMapper
    {
        public static ApplicationLogResponseDto ToDto(this ApplicationLog log)
        {
            return new ApplicationLogResponseDto
            {
                ApplicationLogId = log.ApplicationLogId,
                AuditLogId = log.AuditLogId,
                Message = log.Message,
                Exception = log.Exception,
                StackTrace = log.StackTrace
            };
        }

        public static ApplicationLog ToEntity(this ApplicationLogRequestDto request)
        {
            return new ApplicationLog
            {
                AuditLogId = request.AuditLogId,
                Message = request.Message,
                Exception = request.Exception,
                StackTrace = request.StackTrace
            };
        }
    }
}
