using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Net;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository // Create and read only, no update or delete operations for audit logs
    {
        public Task<OperationResult<long>> AddAuditLogAsync(AuditLog auditLog);
        public Task<OperationResult<AuditLog?>> FindAsync(long auditLogId);
        public Task<OperationResult<AuditLog?>> FindByCorrelationIdAsync(Guid correlationId);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByUserIdAsync(int userId,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByActionTypeAsync(AuditActionType actionType,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByEndpointUrlAsync(string endpointUrl,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByHttpStatusCodeAsync(HttpStatusCode httpStatusCode,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByIpAddressAsync(string ipAddress,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByCreatedBeforeAsync(DateTime dateTime,
            PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<AuditLog>>> GetPagedByCreatedAfterAsync(DateTime dateTime,
            PaginationRequest paginationRequest);
    }
}
