using SMS.Contracts.Common;
using SMS.Contracts.Requests.AuditLogs;
using SMS.Contracts.Responses;
using SMS.Shared.Enums;
using System.Net;

namespace SMS.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        public Task<int> AddAsync(AuditLogRequestDto requestDto);
        public Task<AuditLogResponseDto> GetAsync(long auditLogId);
        public Task<AuditLogResponseDto> GetByCorrelationIdAsync(Guid correlationId);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByUserIdAsync(int userId, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByActionTypeAsync(AuditActionType actionType, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByEndpointAsync(string endpointUrl, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByHttpStatusCodeAsync(HttpStatusCode httpStatusCode, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedByIpAddressAsync(string ipAddress, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedCreatedBeforeAsync(DateTime dateTime, PaginationRequest paginationRequest);
        public Task<IReadOnlyList<PaginationResponse<AuditLogResponseDto>>> GetPagedCreatedAfterAsync(DateTime dateTime, PaginationRequest paginationRequest);
    }
}
