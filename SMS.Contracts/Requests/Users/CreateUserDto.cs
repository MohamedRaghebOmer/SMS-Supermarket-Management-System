using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Users
{
    public sealed record CreateUserDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "PersonId must be greater than 0.")]
        public int PersonId { get; init; }


        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; init; }


        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; init; }


        [Range(1, int.MaxValue, ErrorMessage = "RoleId must be greater than 0.")]
        public int RoleId { get; init; }
    }
}
