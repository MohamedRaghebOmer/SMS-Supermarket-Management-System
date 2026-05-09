using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Enums;
using System.Net;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository // Create and read only, no update or delete operations for audit logs
    {
        public Task<OperationResult<int>> AddAuditLogAsync(AuditLog auditLog);
        public Task<OperationResult<AuditLog?>> GetAsync(int auditLogId);
        public Task<OperationResult<AuditLog?>> GetByCorrelationIdAsync(Guid correlationId);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetPagedAsync(int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetByUserIdPagedAsync(int userId, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetByActionTypePagedAsync(AuditActionType actionType, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetByEndpointUrlPagedAsync(string endpointUrl, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetByHttpStatusCodePagedAsync(HttpStatusCode httpStatusCode, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetByIpAddressPagedAsync(string ipAddress, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetCreatedBeforePagedAsync(DateTime dateTime, int pageNumber, int pageSize);
        public Task<OperationResult<IReadOnlyList<AuditLog>>> GetCreatedAfterPagedAsync(DateTime dateTime, int pageNumber, int pageSize);
    }
}
