using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public sealed record RoleEntityPermissionsResponseDto
    {
        public int RoleId { get; init; }
        public SystemEntity Entity { get; init; }
        public int PermissionsMask { get; init; }
    }
}
