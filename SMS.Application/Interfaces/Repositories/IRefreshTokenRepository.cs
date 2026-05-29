using SMS.Application.Common.Results;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<OperationResult<bool>> AddAsync(RefreshToken refreshToken);
        Task<OperationResult<bool>> IsValidRefreshTokenByUsernameAsync(Guid refreshTokenId, string username);
        Task<OperationResult<bool>> RevokeAsync(Guid refreshToken);
        Task<OperationResult<bool>> RevokeByUsernameAsync(Guid refreshTokenId, string username);
        Task<OperationResult<bool>> HasValidRefreshTokenAsync(int userId);
        Task<OperationResult<IReadOnlyList<RefreshToken>>> FindValidTokensByUsername(string username);
    }
}
