using SMS.Application.Common.Results;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<OperationResult<bool>> AddAsync(RefreshToken refreshToken);
        Task<OperationResult<bool>> IsValidRefreshTokenByUsernameAsync(string tokenHash, string username);
        Task<OperationResult<bool>> DoesTokenBelongToUserAsync(string tokenHash, string username);
        Task<OperationResult<bool>> RevokeAsync(string refreshToken);
    }
}
