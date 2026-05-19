using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public required string Username { get; set; } = string.Empty;

        [Required]
        public required string Password { get; set; } = string.Empty;
    }
}
