using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Products;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;
using System;

namespace SMS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public ProductService(IProductRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateProductRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);


            if (dto.SellPrice < dto.CostPrice)
                throw new ArgumentException("Sell price must be greater than or equal to cost price.");
            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<ProductResponseDto> GetByIdAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.FindByIdAsync(productId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetByCategoryIdAsync(int categoryId,
            PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(categoryId);
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByCategoryIdAsync(categoryId, request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<ProductResponseDto> GetByNameAsync(string productName)
        {
            StringGuard.AgainstNullOrWhiteSpace(productName, nameof(productName));

            var result = await _repo.FindByNameAsync(productName);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<ProductResponseDto> GetBySkuAsync(string sku)
        {
            StringGuard.AgainstNullOrWhiteSpace(sku, nameof(sku));

            var result = await _repo.FindBySkuAsync(sku);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedByUnitIdAsync(int unitId,
            PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(unitId);
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByUnitIdAsync(unitId, request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedByDiscountRangeAsync(
            PaginationRequest request, decimal minPercent, decimal maxPercent)
        {
            _validationHelper.ValidatePagination(request);
            if (minPercent < 0 || maxPercent < 0 || minPercent > maxPercent)
                throw new ArgumentException("Invalid discount percentage range.");

            var result = await _repo.GetPagedByDiscountRangeAsync(request, minPercent, maxPercent);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request,
            bool isActive)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByIsActiveAsync(request, isActive);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedByCreatedAtRangeAsync(
            PaginationRequest request, DateTime from, DateTime to)
        {
            _validationHelper.ValidatePagination(request);
            SMS.Shared.Guards.DateGuard.AgainstInvalidDateRange(from, to, nameof(from), nameof(to));

            var result = await _repo.GetPagedByCreatedAtRangeAsync(request, from, to);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ProductResponseDto>> GetPagedByUpdatedAtRangeAsync(
            PaginationRequest request, DateTime from, DateTime to)
        {
            _validationHelper.ValidatePagination(request);
            SMS.Shared.Guards.DateGuard.AgainstInvalidDateRange(from, to, nameof(from), nameof(to));

            var result = await _repo.GetPagedByUpdatedAtRangeAsync(request, from, to);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<decimal> GetDiscountPercentAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.GetDiscountPercentAsync(productId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<Guid?> GetImageGuidAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.GetImageGuidAsync(productId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(int productId, UpdateProductRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.SellPrice < dto.CostPrice)
                throw new ArgumentException("Sell price must be greater than or equal to cost price.");

            var result = await _repo.UpdateAsync(dto.ToEntity(productId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeactivateAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.DeactivateAsync(productId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ActivateAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.ActivateAsync(productId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        private PaginationResponse<ProductResponseDto> BuildPagination(
            OperationResult<PaginationResponse<Product>> result)
        {
            return new PaginationResponse<ProductResponseDto>
            {
                Items = result.Data!.Items.Select(p => p.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }
    }
}