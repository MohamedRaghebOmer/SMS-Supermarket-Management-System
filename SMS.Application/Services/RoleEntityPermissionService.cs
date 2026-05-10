using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Shared.Enums;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class RoleEntityPermissionService : IRoleEntityPermissionService
    {
        private readonly IRoleEntityPermissionRepository _repo;

        public RoleEntityPermissionService(IRoleEntityPermissionRepository repo)
        {
            _repo = repo;
        }


        public async Task<bool> AddAsync(RoleEntityPermissionsRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(dto.RoleId);
            NumericGuard.AgainstNegativeNumber(dto.PermissionsMask, nameof(dto.PermissionsMask));

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<IReadOnlyList<RoleEntityPermissionsResponseDto>> GetByRoleIdAsync(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            var result = await _repo.GetByRoleIdAsync(roleId);
            result.ThrowIfNotSuccess();

            return result.Data.Select(permission => permission.ToDto()).ToList();
        }

        public async Task<IReadOnlyList<RoleEntityPermissionsResponseDto>> GetByEntityAsync(SystemEntity entity)
        {
            var result = await _repo.GetByEntityAsync(entity);
            result.ThrowIfNotSuccess();

            return result.Data.Select(permission => permission.ToDto()).ToList();
        }

        public async Task<int> GetPermissionsMaskAsync(int roleId, SystemEntity entity)
        {
            NumericGuard.AgainstInvalidId(roleId);

            var result = await _repo.GetPermissionsMaskAsync(roleId, entity);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdatePermissionsMaskAsync(int roleId, SystemEntity entity, int permissionMask)
        {
            NumericGuard.AgainstInvalidId(roleId);
            NumericGuard.AgainstNegativeNumber(permissionMask, nameof(permissionMask));

            var result = await _repo.UpdatePermissionsMaskAsync(roleId, entity, permissionMask);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity)
        {
            NumericGuard.AgainstInvalidId(roleId);

            var result = await _repo.DeleteByRoleAndEntityAsync(roleId, entity);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> HasPermissionAsync(int roleId, SystemEntity entity, PermissionAction action)
        {
            NumericGuard.AgainstInvalidId(roleId);

            var result = await _repo.GetPermissionsMaskAsync(roleId, entity);
            result.ThrowIfNotSuccess();
            return (result.Data & (int)action) == (int)action;
        }
    }
}
