using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IRoleEntityPermissionRepository
    {
        public Task<OperationResult<bool>> AddAsync(RoleEntityPermission entity);
        public Task<OperationResult<IReadOnlyList<RoleEntityPermission>>>
            GetByRoleIdAsync(int roleId);
        public Task<OperationResult<IReadOnlyList<RoleEntityPermission>>>
            GetByEntityAsync(SystemEntity entity);
        public Task<OperationResult<int>> GetPermissionMaskAsync(int roleId, SystemEntity entity);
        public Task<OperationResult<bool>> UpdatePermissionMaskAsync(int roleId, SystemEntity entity, int permissionMask);
        public Task<OperationResult<bool>> DeleteByRoleIdAsync(int roleId);
        public Task<OperationResult<bool>> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity);
    }
}
