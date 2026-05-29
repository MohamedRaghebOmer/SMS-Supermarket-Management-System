using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Users
{
    public class ChangePasswordDto
    {
        [Required]
        public required string OldPassword { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string NewPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string ConfirmNewPassword { get; set; }
    }
}
