using SMS.Contracts.Requests.Roles;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class RoleMapper
    {
        public static Role ToEntity(this CreateRoleRequestDto dto)
        {
            return new Role(dto.RoleName, dto.RoleDescription, true);
        }

        public static Role ToEntity(this UpdateRoleRequestDto dto, int roleId)
        {
            return new Role(roleId, dto.RoleName, dto.RoleDescription, dto.IsActive);
        }

        public static RoleResponseDto ToDto(this Role entity)
        {
            return new RoleResponseDto
            {
                RoleId = entity.RoleId,
                RoleName = entity.RoleName,
                RoleDescription = entity.RoleDescription,
                IsActive = entity.IsActive
            };
        }
    }
}
