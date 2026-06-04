using SMS.Contracts.Requests.Categories;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<int> AddAsync(CreateCategoryRequestDto dto);
        Task<CategoryResponseDto> GetByIdAsync(int categoryId);
        Task<CategoryResponseDto> GetByNameAsync(string categoryName);
        Task<PaginationResponse<CategoryResponseDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginationResponse<CategoryResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive);
        Task<bool> IsActive(int categoryId);
        Task<bool> UpdateAsync(int categoryId, UpdateCategoryRequestDto dto);
        Task<bool> ActivateAsync(int categoryId);
        Task<bool> DeactivateAsync(int categoryId);
    }
}