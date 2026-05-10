using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public static class RoleEntityPermissionMapper
    {
        public static RoleEntityPermission ToEntity(this RoleEntityPermissionsRequestDto dto)
        {
            return new RoleEntityPermission(dto.RoleId, dto.Entity, dto.PermissionsMask);
        }

        public static RoleEntityPermissionsResponseDto ToDto(this RoleEntityPermission entity)
        {
            return new RoleEntityPermissionsResponseDto
            {
                RoleId = entity.RoleId,
                Entity = entity.Entity,
                PermissionsMask = entity.PermissionsMask
            };
        }
    }
}