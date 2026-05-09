using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public class RoleEntityPermissionResponseDto
    {
        public int RoleId { get; set; }
        public SystemEntity Entity { get; set; }
        public int PermissionMask { get; set; }
    }
}
