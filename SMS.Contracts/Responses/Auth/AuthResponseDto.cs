namespace SMS.Contracts.Responses.Auth
{
    public sealed record AuthResponseDto
    {
        public string? AccessToken { get; init; } = null;
        public string? RefreshToken { get; init; } = null;
    }
}
