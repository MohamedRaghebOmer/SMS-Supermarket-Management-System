using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Domain.Entities;
using SMS.Shared.Constants;
using SMS.Shared.Guards;
using System.Security.Cryptography;

namespace SMS.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokensRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStringHelper _stringHelper;

        public RefreshTokenService(IRefreshTokenRepository refreshTokensRepository,
            IUserRepository userRepository, IStringHelper stringHelper)
        {
            _refreshTokensRepository = refreshTokensRepository;
            _userRepository = userRepository;
            _stringHelper = stringHelper;
        }

        public async Task<string> GenerateRefreshTokenByUsernameAsync(string username)
        {
            username = username.Trim();
            StringGuard.AgainstNullOrWhiteSpace(username, nameof(username));

            var user = await _userRepository.FindByUsernameAsync(username);
            user.ThrowIfNotSuccess();
            user.ThrowNotFoundIfDataNull();

            var token = GenerateRefreshToken();
            var tokenHash = _stringHelper.Hash(token);
            var refreshToken = new RefreshToken(Guid.NewGuid(), user.Data.UserId, tokenHash,
                DateTime.UtcNow.AddDays(Constants.RefreshTokenPeriod),
                DateTime.UtcNow, null, false);

            var result = await _refreshTokensRepository.AddAsync(refreshToken);
            result.ThrowIfNotSuccess();


            return token;
        }

        public async Task<string> GenerateRefreshTokenByUserIdAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var token = GenerateRefreshToken();
            var tokenHash = _stringHelper.Hash(token);
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, tokenHash,
                DateTime.UtcNow.AddDays(Constants.RefreshTokenPeriod),
                DateTime.UtcNow, null, false);

            // 'AddAsync' validates the user exists before adding the refresh token
            var result = await _refreshTokensRepository.AddAsync(refreshToken);
            result.ThrowIfNotSuccess();

            return token;
        }

        public async Task<bool> IsValidRefreshTokenByUsernameAsync(Guid refreshTokenId, string username)
        {
            username = username.Trim();

            if (refreshTokenId == Guid.Empty || string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var result = await _refreshTokensRepository.IsValidRefreshTokenByUsernameAsync(
                refreshTokenId, username);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> RevokeRefreshTokenAsync(Guid refreshTokenId)
        {
            if (refreshTokenId == Guid.Empty)
            {
                return false;
            }

            var result = await _refreshTokensRepository.RevokeAsync(refreshTokenId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> RevokeRefreshTokenByUsernameAsync(Guid refreshTokenId, string username)
        {
            username = username.Trim();
            StringGuard.AgainstNullOrWhiteSpace(username, nameof(username));

            if (refreshTokenId == Guid.Empty)
            {
                throw new ArgumentException("Refresh token ID cannot be empty.", nameof(refreshTokenId));
            }

            var result = await _refreshTokensRepository.RevokeByUsernameAsync(refreshTokenId, username);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> HasValidRefreshToken(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _refreshTokensRepository.HasValidRefreshTokenAsync(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }


        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}