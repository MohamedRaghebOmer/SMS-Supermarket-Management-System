using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<OperationResult<int>> AddAsync(Role role);
        Task<OperationResult<Role?>> FindByIdAsync(int roleId);
        Task<OperationResult<Role?>> FindByNameAsync(string roleName);
        Task<OperationResult<PaginationResponse<Role>>> GetPagedAsync(PaginationRequest request);
        Task<OperationResult<PaginationResponse<Role>>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive);
        Task<OperationResult<PaginationResponse<Role>>> GetPagedByCreatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to);
        Task<OperationResult<string>> FindRoleNameByIdAsync(int roleId);
        Task<OperationResult<bool>> IsActive(int roleId);
        Task<OperationResult<bool>> UpdateAsync(Role role);
        Task<OperationResult<bool>> ActivateAsync(int roleId);
        Task<OperationResult<bool>> DeactivateAsync(int roleId);
    }
}
