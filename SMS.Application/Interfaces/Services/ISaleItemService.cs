using SMS.Contracts.Requests.SaleItems;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface ISaleItemService
    {
        Task<int> AddAsync(CreateSaleItemRequestDto dto);
        Task<SaleItemResponseDto> GetByIdAsync(int saleItemId);
        Task<PaginationResponse<SaleItemResponseDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginationResponse<SaleItemResponseDto>> GetPagedBySaleIdAsync(int saleId, PaginationRequest request);
        Task<PaginationResponse<SaleItemResponseDto>> GetPagedByProductIdAsync(int productId, PaginationRequest request);
        Task<SaleItemResponseDto> GetBySaleIdAndProductIdAsync(int saleId, int productId);
        Task<decimal> GetLineTotalByIdAsync(int saleItemId);
        Task<bool> UpdateAsync(int saleItemId, UpdateSaleItemRequestDto dto);
        Task<bool> DeleteAsync(int saleItemId);
    }
}
