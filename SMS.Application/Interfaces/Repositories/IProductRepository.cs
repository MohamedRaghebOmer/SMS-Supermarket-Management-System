using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<OperationResult<int>> AddAsync(Product product);
        Task<OperationResult<Product?>> FindByIdAsync(int productId);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedByCategoryIdAsync(int categoryId,
            PaginationRequest request);

        Task<OperationResult<Product?>> FindByNameAsync(string productName);
        Task<OperationResult<Product?>> FindBySkuAsync(string sku);
        Task<OperationResult<PaginationResponse<Product>>> GetPagedByUnitIdAsync(int unitId, PaginationRequest request);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedByDiscountRangeAsync(PaginationRequest request,
            decimal minPercent, decimal maxPercent);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedByIsActiveAsync(PaginationRequest request,
            bool isActive);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedByCreatedAtRangeAsync(PaginationRequest request,
            DateTime from, DateTime to);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedByUpdatedAtRangeAsync(PaginationRequest request,
            DateTime from, DateTime to);

        Task<OperationResult<PaginationResponse<Product>>> GetPagedAsync(PaginationRequest request);

        Task<OperationResult<decimal>> GetDiscountPercentAsync(int productId);
        Task<OperationResult<Guid?>> GetImageGuidAsync(int productId);
        Task<OperationResult<bool>> UpdateAsync(Product product);
        Task<OperationResult<bool>> ActivateAsync(int productId);
        Task<OperationResult<bool>> DeactivateAsync(int productId);
    }
}