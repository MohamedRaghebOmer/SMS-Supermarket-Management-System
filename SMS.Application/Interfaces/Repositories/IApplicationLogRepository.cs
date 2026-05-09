using Microsoft.Extensions.Logging;
using SMS.Application.Common.Results;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IApplicationLogRepository
    {
        public Task<OperationResult<int>> AddAsync(ApplicationLog log);
        public Task<OperationResult<ApplicationLog?>> GetAsync(int id);
        public Task<OperationResult<IReadOnlyList<ApplicationLogResponseDto>>> GetByAuditLogIdAsync(int auditLogId);
        public Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedAsync(int page, int pageSize);
        public Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedByLogLevelAsync(LogLevel logLevel, int page, int pageSize);
        public Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, int page, int pageSize);
    }
}
