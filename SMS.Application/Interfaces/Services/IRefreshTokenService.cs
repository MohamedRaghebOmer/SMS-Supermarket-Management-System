namespace SMS.Application.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenByUsernameAsync(string username);
        Task<string> GenerateRefreshTokenByUserIdAsync(int userId);
        Task<bool> IsValidRefreshTokenByUsernameAsync(Guid refreshTokenId, string username);
        Task<bool> RevokeRefreshTokenByUsernameAsync(Guid refreshTokenId, string username);
        Task<bool> RevokeRefreshTokenAsync(Guid refreshTokenId);
        Task<bool> HasValidRefreshToken(int userId);
    }
}
