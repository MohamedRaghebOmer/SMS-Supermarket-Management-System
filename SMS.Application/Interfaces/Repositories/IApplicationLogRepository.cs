using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using LogLevel = SMS.Shared.Enums.LogLevel;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IApplicationLogRepository
    {
        public Task<OperationResult<int>> AddAsync(ApplicationLog log);
        public Task<OperationResult<ApplicationLog?>> FindAsync(int id);
        public Task<OperationResult<ApplicationLog?>> FindByAuditLogIdAsync(long auditLogId);
        public Task<OperationResult<PaginationResponse<ApplicationLog>>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<ApplicationLog>>> GetPagedByLogLevelAsync(LogLevel logLevel, PaginationRequest paginationRequest);
        public Task<OperationResult<PaginationResponse<ApplicationLog>>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, PaginationRequest paginationRequest);
    }
}
