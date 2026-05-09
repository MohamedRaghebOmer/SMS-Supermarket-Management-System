using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Services
{
    public interface IRoleEntityPermissionService
    {
        public Task<bool> AddAsync(RoleEntityPermissionRequestDto dto);
        public Task<IReadOnlyList<RoleEntityPermissionResponseDto>> GetByRoleIdAsync(int roleId);
        public Task<IReadOnlyList<RoleEntityPermissionResponseDto>> GetByEntityAsync(SystemEntity entity);
        public Task<int> GetPermissionMaskAsync(int roleId, SystemEntity entity);
        public Task<bool> UpdatePermissionMaskAsync(int roleId, SystemEntity entity, int permissionMask);
        public Task<bool> DeleteByRoleIdAsync(int roleId);
        public Task<bool> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity);
        public Task<bool> HasPermissionAsync(int roleId, SystemEntity entity, PermissionAction action);
    }
}
