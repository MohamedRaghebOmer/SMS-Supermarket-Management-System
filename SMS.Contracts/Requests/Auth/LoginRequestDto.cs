using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Auth
{
    public sealed record LoginRequestDto
    {
        [Required]
        public required string Username { get; init; } = string.Empty;

        [Required]
        public required string Password { get; init; } = string.Empty;
    }
}
