namespace SMS.Contracts.Responses.Auth
{
    public sealed record class LoginResultDto
    {
        public enum LoginResultStatus
        {
            Success,
            InvalidCredentials,
            InactiveAccount,
            AlreadyLoggedIn
        }

        public string? AccessToken { get; set; } = null;
        public string? RefreshToken { get; set; } = null;
        public LoginResultStatus Status { get; set; } = LoginResultStatus.InvalidCredentials;
        public string? Message { get; set; } = null;
    }
}
