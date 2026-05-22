namespace SMS.Contracts.Responses.Auth
{
    public class AuthResponseDto
    {
        public string? AccessToken { get; set; } = null;
        public string? RefreshToken { get; set; } = null;
    }
}
