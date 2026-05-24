using SMS.Contracts.Requests.Users;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class UserMapper
    {
        public static User ToEntity(this CreateUserDto dto)
        {
            return new User(
                personId: dto.PersonId,
                username: dto.Username,
                passwordHash: dto.Password,
                roleId: dto.RoleId,
                isActive: true,
                lastLoginAt: null,
                createdAt: DateTime.UtcNow,
                lastUpdatedAt: null);
        }

        public static User ToEntity(this UpdateUserDto dto)
        {
            return new User(
                personId: dto.PersonId,
                username: dto.Username,
                passwordHash: dto.Password,
                roleId: dto.RoleId,
                isActive: dto.IsActive,
                lastLoginAt: null,
                createdAt: DateTime.UtcNow,
                lastUpdatedAt: DateTime.UtcNow);
        }

        public static UserResponseDto ToDto(this User entity)
        {
            return new UserResponseDto
            {
                UserId = entity.UserId,
                PersonId = entity.PersonId,
                UserName = entity.Username,
                RoleId = entity.RoleId,
                IsActive = entity.IsActive
            };
        }
    }
}
