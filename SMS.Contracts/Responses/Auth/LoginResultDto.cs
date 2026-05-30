namespace SMS.Contracts.Responses.Auth
{
    public sealed record LoginResultDto
    {
        public enum LoginResultStatus
        {
            Success,
            InvalidCredentials,
            InactiveAccount,
            AlreadyLoggedIn
        }

        public string? AccessToken { get; init; } = null;
        public string? RefreshToken { get; init; } = null;
        public LoginResultStatus Status { get; init; } = LoginResultStatus.InvalidCredentials;
        public string? Message { get; init; } = null;
    }
}
