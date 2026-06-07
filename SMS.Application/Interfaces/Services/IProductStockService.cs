using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IProductStockService
    {
        Task<PaginationResponse<ProductStock>> GetPagedAsync(PaginationRequest request);
        Task<ProductStock> GetByIdAsync(int productId);
        Task<decimal> GetQuantityOnHandAsync(int productId);
        Task<decimal> GetReorderLevelAsync(int productId);
        Task<bool> UpdateReorderLevelAsync(int productId, decimal reorderLevel);
    }
}
