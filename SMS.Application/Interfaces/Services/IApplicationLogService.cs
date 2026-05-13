using SMS.Contracts.Requests.ApplicationLogs;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IApplicationLogService
    {
        public Task<int> AddAsync(ApplicationLogRequestDto dto);
        public Task<ApplicationLogResponseDto> GetAsync(int id);
        public Task<IReadOnlyList<ApplicationLogResponseDto>> GetByAuditLogIdAsync(int auditLogId);
        public Task<IReadOnlyList<ApplicationLogResponseDto>> GetPagedAsync(PaginationRequest pagination);
        public Task<IReadOnlyList<ApplicationLogResponseDto>> GetPagedByLogLevelAsync(Shared.Enums.LogLevel logLevel, PaginationRequest pagination);
        public Task<IReadOnlyList<ApplicationLogResponseDto>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, PaginationRequest pagination);
    }
}
