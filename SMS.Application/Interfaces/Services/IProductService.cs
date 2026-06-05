using SMS.Contracts.Requests.Products;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<int> AddAsync(CreateProductRequestDto dto);
        Task<ProductResponseDto> GetByIdAsync(int productId);
        Task<PaginationResponse<ProductResponseDto>> GetByCategoryIdAsync(int categoryId, PaginationRequest request);
        Task<ProductResponseDto> GetByNameAsync(string productName);
        Task<ProductResponseDto> GetBySkuAsync(string sku);
        Task<PaginationResponse<ProductResponseDto>> GetPagedByUnitIdAsync(int unitId, PaginationRequest request);
        Task<PaginationResponse<ProductResponseDto>> GetPagedByDiscountRangeAsync(PaginationRequest request, decimal minPercent, decimal maxPercent);
        Task<PaginationResponse<ProductResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive);
        Task<PaginationResponse<ProductResponseDto>> GetPagedByCreatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to);
        Task<PaginationResponse<ProductResponseDto>> GetPagedByUpdatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to);
        Task<PaginationResponse<ProductResponseDto>> GetPagedAsync(PaginationRequest request);
        Task<decimal> GetDiscountPercentAsync(int productId);
        Task<Guid?> GetImageGuidAsync(int productId);
        Task<bool> UpdateAsync(int productId, UpdateProductRequestDto dto);
        Task<bool> DeactivateAsync(int productId);
        Task<bool> ActivateAsync(int productId);
    }
}
