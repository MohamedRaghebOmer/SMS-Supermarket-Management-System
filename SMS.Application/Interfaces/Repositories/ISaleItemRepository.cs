using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ISaleItemRepository
    {
        Task<OperationResult<int>> AddAsync(SaleItem saleItem);
        Task<OperationResult<SaleItem?>> FindByIdAsync(int saleItemId);
        Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedBySaleIdAsync(int saleId, PaginationRequest request);
        Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedAsync(PaginationRequest request);
        Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedByProductIdAsync(int productId, PaginationRequest request);
        Task<OperationResult<SaleItem?>> FindBySaleIdAndProductIdAsync(int saleId, int productId);
        Task<OperationResult<decimal>> GetLineTotalByIdAsync(int saleItemId);
        Task<OperationResult<bool>> UpdateAsync(SaleItem saleItem);
        Task<OperationResult<bool>> DeleteAsync(int saleItemId);
    }
}
