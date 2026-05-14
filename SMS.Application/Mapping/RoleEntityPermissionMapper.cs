using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public static class RoleEntityPermissionMapper
    {
        public static RoleEntityPermissions ToEntity(this RoleEntityPermissionsRequestDto dto)
        {
            return new RoleEntityPermissions(dto.RoleId, dto.Entity, dto.PermissionsMask);
        }

        public static RoleEntityPermissionsResponseDto ToDto(this RoleEntityPermissions entity)
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