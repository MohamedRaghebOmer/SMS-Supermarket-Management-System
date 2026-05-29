using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Services
{
    public interface IRoleEntityPermissionService
    {
        public Task<bool> AddAsync(RoleEntityPermissionsRequestDto dto);
        /// <summary>
        /// Gets the role identifier for the specified user identifier.
        /// </summary>
        public Task<int> GetRoleIdByUserIdAsync(int userId);
        public Task<IReadOnlyList<RoleEntityPermissionsResponseDto>> GetByRoleIdAsync(int roleId);
        public Task<IReadOnlyList<RoleEntityPermissionsResponseDto>> GetByEntityAsync(SystemEntity entity);
        public Task<int> GetPermissionsMaskAsync(int roleId, SystemEntity entity);
        public Task<bool> UpdatePermissionsMaskAsync(int roleId, SystemEntity entity, int permissionsMask);
        public Task<bool> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity);
        public Task<bool> HasPermissionAsync(int roleId, SystemEntity entity, PermissionAction action);
    }
}
