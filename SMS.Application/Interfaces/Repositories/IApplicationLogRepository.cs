using SMS.Application.Common.Results;
using SMS.Contracts.Common;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IApplicationLogRepository
    {
        public Task<OperationResult<int>> AddAsync(ApplicationLog log);
        public Task<IReadOnlyList<ApplicationLog>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<ApplicationLog?> GetAsync(int id);
    }
}
