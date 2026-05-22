using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }

        [Required]
        public required string Username { get; set; }
    }
}
