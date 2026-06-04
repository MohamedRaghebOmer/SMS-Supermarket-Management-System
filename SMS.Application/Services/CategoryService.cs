using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Categories;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public CategoryService(ICategoryRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateCategoryRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            StringGuard.AgainstNullOrWhiteSpace(dto.CategoryName, nameof(dto.CategoryName));

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int categoryId)
        {
            NumericGuard.AgainstInvalidId(categoryId);

            var result = await _repo.FindByIdAsync(categoryId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<CategoryResponseDto> GetByNameAsync(string categoryName)
        {
            StringGuard.AgainstNullOrWhiteSpace(categoryName, nameof(categoryName));

            var result = await _repo.FindByNameAsync(categoryName);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<CategoryResponseDto>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return BuildPaginationResponse(result);
        }

        public async Task<PaginationResponse<CategoryResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request,
            bool isActive)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByIsActiveAsync(request, isActive);
            result.ThrowIfNotSuccess();

            return BuildPaginationResponse(result);
        }

        public async Task<bool> IsActive(int categoryId)
        {
            NumericGuard.AgainstInvalidId(categoryId);

            var result = await _repo.IsActive(categoryId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(int categoryId, UpdateCategoryRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(categoryId);
            StringGuard.AgainstNullOrWhiteSpace(dto.CategoryName, nameof(dto.CategoryName));

            var result = await _repo.UpdateAsync(dto.ToEntity(categoryId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ActivateAsync(int categoryId)
        {
            NumericGuard.AgainstInvalidId(categoryId);

            var result = await _repo.ActivateAsync(categoryId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeactivateAsync(int categoryId)
        {
            NumericGuard.AgainstInvalidId(categoryId);

            var result = await _repo.DeactivateAsync(categoryId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        private PaginationResponse<CategoryResponseDto> BuildPaginationResponse(
            OperationResult<PaginationResponse<Category>> result)
        {
            return new PaginationResponse<CategoryResponseDto>
            {
                Items = result.Data!.Items.Select(c => c.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }
    }
}