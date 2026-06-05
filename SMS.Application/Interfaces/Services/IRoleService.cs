using SMS.Contracts.Requests.Roles;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<int> AddAsync(CreateRoleRequestDto dto);
        Task<RoleResponseDto> GetByIdAsync(int roleId);
        Task<RoleResponseDto> GetByNameAsync(string roleName);
        Task<PaginationResponse<RoleResponseDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginationResponse<RoleResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive);
        Task<PaginationResponse<RoleResponseDto>> GetPagedByCreatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to);
        Task<string> GetRoleNameByIdAsync(int roleId);
        Task<bool> IsActive(int roleId);
        Task<bool> UpdateAsync(int roleId, UpdateRoleRequestDto dto);
        Task<bool> ActivateAsync(int roleId);
        Task<bool> DeactivateAsync(int roleId);
    }
}
