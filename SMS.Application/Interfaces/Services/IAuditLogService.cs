using SMS.Contracts.Requests.AuditLogs;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Net;

namespace SMS.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        public Task<long> AddAsync(AuditLogRequestDto requestDto);
        public Task<AuditLogResponseDto> GetAsync(long auditLogId);
        public Task<AuditLogResponseDto> GetByCorrelationIdAsync(Guid correlationId);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedByUserIdAsync(int userId,
            PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedByActionTypeAsync(AuditActionType actionType,
            PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedByEndpointAsync(string endpointUrl,
            PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedByHttpStatusCodeAsync(HttpStatusCode httpStatusCode, PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedByIpAddressAsync(string ipAddress,
            PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedCreatedBeforeAsync(DateTime dateTime,
            PaginationRequest paginationRequest);
        public Task<PaginationResponse<AuditLogResponseDto>> GetPagedCreatedAfterAsync(DateTime dateTime,
            PaginationRequest paginationRequest);
    }
}
