using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IUnitRepository
    {
        Task<OperationResult<int>> AddAsync(Unit unit);
        Task<OperationResult<Unit?>> FindByIdAsync(int unitId);
        Task<OperationResult<Unit?>> FindByNameAsync(string unitName);
        Task<OperationResult<Unit?>> FindBySymbolAsync(string symbol);
        Task<OperationResult<PaginationResponse<Unit>>> GetPagedByIsDecimalAsync(PaginationRequest request, bool isDecimal);
        Task<OperationResult<bool>> UpdateAsync(Unit unit);
    }
}
