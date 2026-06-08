using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.ReturnItems;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class ReturnItemService : IReturnItemService
    {
        private readonly IReturnItemRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public ReturnItemService(IReturnItemRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateReturnItemRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = dto.ToEntity();

            var expectedLineTotal = entity.Quantity * entity.UnitPrice;
            if (expectedLineTotal != entity.LineTotal)
            {
                throw new ArgumentException("LineTotal must equal Quantity * UnitPrice.");
            }

            var result = await _repo.AddAsync(entity);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<ReturnItemResponseDto> GetByIdAsync(int returnItemId)
        {
            NumericGuard.AgainstInvalidId(returnItemId);

            var result = await _repo.FindByIdAsync(returnItemId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<ReturnItemResponseDto>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);
            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ReturnItemResponseDto>> GetPagedByReturnIdAsync(int returnId, PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(returnId);
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByReturnIdAsync(returnId, request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        public async Task<PaginationResponse<ReturnItemResponseDto>> GetPagedByProductIdAsync(int productId, PaginationRequest request)
        {
            NumericGuard.AgainstInvalidId(productId);
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByProductIdAsync(productId, request);
            result.ThrowIfNotSuccess();

            return BuildPagination(result);
        }

        private PaginationResponse<ReturnItemResponseDto> BuildPagination(OperationResult<PaginationResponse<ReturnItem>> result)
        {
            return new PaginationResponse<ReturnItemResponseDto>
            {
                Items = result.Data!.Items.Select(i => i.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }
    }
}
