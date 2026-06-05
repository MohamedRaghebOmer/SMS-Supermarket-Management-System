using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.SaleItems;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class SaleItemService : ISaleItemService
    {
        private readonly ISaleItemRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public SaleItemService(ISaleItemRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateSaleItemRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<SaleItemResponseDto> GetByIdAsync(int saleItemId)
        {
            NumericGuard.AgainstInvalidId(saleItemId);

            var result = await _repo.FindByIdAsync(saleItemId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<SaleItemResponseDto>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<SaleItemResponseDto>> GetPagedBySaleIdAsync(int saleId, PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(saleId);
            _validationHelper.ValidatePagination(request);
            var result = await _repo.GetPagedBySaleIdAsync(saleId, request);
            result.ThrowIfNotSuccess();
            return BuildPagination(result);
        }

        public async Task<PaginationResponse<SaleItemResponseDto>> GetPagedByProductIdAsync(int productId, PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(productId);
            _validationHelper.ValidatePagination(request);
            var result = await _repo.GetPagedByProductIdAsync(productId, request);
            result.ThrowIfNotSuccess();
            return BuildPagination(result);
        }

        public async Task<SaleItemResponseDto> GetBySaleIdAndProductIdAsync(int saleId, int productId)
        {
            NumericGuard.AgainstInvalidId(saleId);
            NumericGuard.AgainstInvalidId(productId);
            var result = await _repo.FindBySaleIdAndProductIdAsync(saleId, productId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();
            return result.Data!.ToDto();
        }

        public async Task<decimal> GetLineTotalByIdAsync(int saleItemId)
        {
            NumericGuard.AgainstInvalidId(saleItemId);
            var result = await _repo.GetLineTotalByIdAsync(saleItemId);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateAsync(int saleItemId, UpdateSaleItemRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var result = await _repo.UpdateAsync(dto.ToEntity(saleItemId));
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> DeleteAsync(int saleItemId)
        {
            NumericGuard.AgainstInvalidId(saleItemId);
            var result = await _repo.DeleteAsync(saleItemId);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        private PaginationResponse<SaleItemResponseDto> BuildPagination(OperationResult<PaginationResponse<SaleItem>> result)
        {
            return new PaginationResponse<SaleItemResponseDto>
            {
                Items = result.Data!.Items.Select(i => i.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }
    }
}
