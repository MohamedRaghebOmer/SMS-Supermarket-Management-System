using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.RoleEntityPermissions
{
    public sealed record RoleEntityPermissionsRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive integer.")]
        public int RoleId { get; init; }

        public SystemEntity Entity { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "PermissionsMask must be a non-negative integer.")]
        public int PermissionsMask { get; init; }
    }
}
