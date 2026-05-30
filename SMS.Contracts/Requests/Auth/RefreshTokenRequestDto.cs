using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Auth
{
    public sealed record RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; init; }

        [Required]
        public required string Username { get; init; }
    }
}
