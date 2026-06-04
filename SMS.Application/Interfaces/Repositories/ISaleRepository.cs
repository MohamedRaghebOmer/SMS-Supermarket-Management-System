using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<OperationResult<int>> AddAsync(Sale sale);
        Task<OperationResult<Sale?>> FindByIdAsync(int saleId);
        Task<OperationResult<PaginationResponse<Sale>>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Sale>>> GetPagedByCashierIdAsync(int cashierId, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Sale>>> GetPagedByCustomerIdAsync(int customerId, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Sale>>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, PaginationRequest paginationRequest);
        Task<OperationResult<bool>> ExistsByIdAsync(int saleId);
    }
}