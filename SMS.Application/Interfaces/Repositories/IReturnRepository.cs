using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IReturnRepository
    {
        Task<OperationResult<int>> AddAsync(Return returnEntity);
        Task<OperationResult<Return?>> FindByIdAsync(int returnId);
        Task<OperationResult<PaginationResponse<Return>>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<OperationResult<IReadOnlyList<Return>>> GetBySaleIdAsync(int saleId);
        Task<OperationResult<PaginationResponse<Return>>> GetPagedByCustomerIdAsync(int customerId, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Return>>> GetPagedByDateRangeAsync(DateTime? startDate, DateTime? endDate, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Return>>> GetPagedByReturnTotalRangeAsync(decimal minTotal, decimal maxTotal, PaginationRequest paginationRequest);
        Task<OperationResult<decimal>> GetReturnTotalByIdAsync(int returnId);
    }
}
