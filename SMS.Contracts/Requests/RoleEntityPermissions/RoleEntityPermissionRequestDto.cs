using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.RoleEntityPermissions
{
    public class RoleEntityPermissionRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive integer.")]
        public int RoleId { get; set; }

        public SystemEntity Entity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "PermissionsMask must be a non-negative integer.")]
        public int PermissionMask { get; set; }
    }
}
