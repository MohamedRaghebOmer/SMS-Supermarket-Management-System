using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public static class RoleEntityPermissionMapper
    {
        public static RoleEntityPermission ToEntity(this RoleEntityPermissionRequestDto dto)
        {
            return new RoleEntityPermission(dto.RoleId, dto.Entity, dto.PermissionMask);
        }

        public static RoleEntityPermissionResponseDto ToDto(this RoleEntityPermission entity)
        {
            return new RoleEntityPermissionResponseDto
            {
                RoleId = entity.RoleId,
                Entity = entity.Entity,
                PermissionMask = entity.PermissionMask
            };
        }
    }
}