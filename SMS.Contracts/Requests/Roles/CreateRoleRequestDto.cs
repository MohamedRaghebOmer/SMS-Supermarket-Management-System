using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Roles
{
    public sealed record CreateRoleRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Role name must be between 1 and 50 characters.")]
        public string RoleName { get; init; } = string.Empty;

        [StringLength(250, ErrorMessage = "Role description cannot exceed 250 characters.")]
        public string? RoleDescription { get; init; }
    }
}
