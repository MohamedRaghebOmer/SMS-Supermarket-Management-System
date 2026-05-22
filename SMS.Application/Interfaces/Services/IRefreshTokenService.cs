namespace SMS.Application.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string username);
        Task<bool> IsValidRefreshTokenByUsernameAsync(string refreshToken, string username);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
