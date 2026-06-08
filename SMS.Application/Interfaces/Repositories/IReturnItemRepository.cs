using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IReturnItemRepository
    {
        Task<OperationResult<int>> AddAsync(ReturnItem returnItem);
        Task<OperationResult<ReturnItem?>> FindByIdAsync(int returnItemId);
        Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedAsync(PaginationRequest request);
        Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedByReturnIdAsync(int returnId, PaginationRequest request);
        Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedByProductIdAsync(int productId, PaginationRequest request);
    }
}
