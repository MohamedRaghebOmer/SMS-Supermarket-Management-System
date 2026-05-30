using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Users
{
    public sealed record ChangePasswordDto
    {
        [Required]
        public required string OldPassword { get; init; }


        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string NewPassword { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string ConfirmNewPassword { get; init; }
    }
}
