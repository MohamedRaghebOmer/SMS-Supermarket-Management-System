using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IProductStockRepository
    {
        Task<OperationResult<PaginationResponse<ProductStock>>> GetPagedAsync(PaginationRequest request);
        Task<OperationResult<ProductStock?>> FindByIdAsync(int productId);
        Task<OperationResult<decimal>> GetQuantityOnHandAsync(int productId);
        Task<OperationResult<decimal>> GetReorderLevelAsync(int productId);
        Task<OperationResult<bool>> UpdateReorderLevelAsync(int productId, decimal reorderLevel);
    }
}
