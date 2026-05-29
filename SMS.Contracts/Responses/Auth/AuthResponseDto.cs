namespace SMS.Contracts.Responses.Auth
{
    public sealed record class AuthResponseDto
    {
        public string? AccessToken { get; set; } = null;
        public string? RefreshToken { get; set; } = null;
    }
}
