using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Domain.Entities;
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

        public async Task<string> GenerateRefreshTokenAsync(string username)
        {
            StringGuard.AgainstNullOrEmpty(username, nameof(username));

            var user = await _userRepository.FindByUsernameAsync(username);

            user.ThrowIfNotSuccess();
            user.ThrowNotFoundIfDataNull();

            var token = _stringHelper.Hash(GenerateRefreshToken());
            var refreshToken = new RefreshToken(Guid.NewGuid(), user.Data.UserId, token, DateTime.UtcNow.AddDays(7),
                DateTime.UtcNow, null, false);

            await _refreshTokensRepository.AddAsync(refreshToken);

            return token;
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public async Task<bool> IsValidRefreshTokenByUsernameAsync(string refreshToken, string username)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(username))
            {
                return false;
            }

            var result = await _refreshTokensRepository.IsValidRefreshTokenByUsernameAsync(
                _stringHelper.Hash(refreshToken.Trim()), username.Trim());

            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            await _refreshTokensRepository.RevokeAsync(_stringHelper.Hash(refreshToken.Trim()));
        }
    }
}