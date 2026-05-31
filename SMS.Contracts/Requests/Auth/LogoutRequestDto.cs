namespace SMS.Contracts.Requests.Auth
{
    public sealed record LogoutRequestDto
    {
        public required string RefreshToken { get; init; }
    }
}
