using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<OperationResult<int>> AddAsync(Category category);
        Task<OperationResult<Category?>> FindByIdAsync(int categoryId);
        Task<OperationResult<Category?>> FindByNameAsync(string categoryName);
        Task<OperationResult<PaginationResponse<Category>>> GetPagedAsync(PaginationRequest request);

        Task<OperationResult<PaginationResponse<Category>>> GetPagedByIsActiveAsync(PaginationRequest request,
            bool isActive);

        Task<OperationResult<bool>> IsActive(int categoryId);
        Task<OperationResult<bool>> UpdateAsync(Category category);
        Task<OperationResult<bool>> ActivateAsync(int categoryId);
        Task<OperationResult<bool>> DeactivateAsync(int categoryId);
    }
}