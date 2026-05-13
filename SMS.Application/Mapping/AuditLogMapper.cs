using SMS.Contracts.Requests.AuditLogs;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public static class AuditLogMapper
    {
        public static AuditLog ToEntity(this AuditLogRequestDto dto)
        {
            return new AuditLog
            {
                UserId = dto.UserId,
                AttemptedLoginIdentifier = dto.AttemptedLoginIdentifier,
                RequestGuid = dto.CorrelationId,
                ActionType = dto.ActionType,
                Endpoint = dto.Endpoint,
                RequestBody = dto.RequestBody,
                ResponseBody = dto.ResponseBody,
                UserAgent = dto.UserAgent,
                HttpStatusCode = dto.HttpStatusCode,
                Duration = dto.Duration,
                IpAddress = dto.IpAddress,
                Details = dto.Details,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuditLogResponseDto ToDto(this AuditLog entity)
        {
            return new AuditLogResponseDto
            {
                AuditLogId = entity.AuditLogId,
                UserId = entity.UserId,
                CorrelationId = entity.RequestGuid,
                ActionType = entity.ActionType,
                Endpoint = entity.Endpoint,
                RequestBody = entity.RequestBody,
                ResponseBody = entity.ResponseBody,
                UserAgent = entity.UserAgent,
                HttpStatusCode = entity.HttpStatusCode,
                Duration = entity.Duration,
                IpAddress = entity.IpAddress,
                Details = entity.Details,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
