using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public class RoleEntityPermissionsResponseDto
    {
        public int RoleId { get; set; }
        public SystemEntity Entity { get; set; }
        public int PermissionsMask { get; set; }
    }
}
