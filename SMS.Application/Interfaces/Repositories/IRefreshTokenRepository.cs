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
        /// <summary>
        /// Finds the valid refresh token hash by username. Returns null if not found or invalid.
        /// Valid means the refresh token is not revoked and not expired. 
        /// The hash is used to compare with the hash of the provided refresh token in the request.
        /// </summary>
        Task<OperationResult<RefreshToken?>> FindValidTokenByUsername(string username);
    }
}
