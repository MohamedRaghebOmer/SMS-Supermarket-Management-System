using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Auth;
using SMS.Contracts.Responses.Auth;
using SMS.Domain.Entities;
using SMS.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRolesRepository _rolesRepo;
        private readonly IConfiguration _configuration;
        private readonly IStringHelper _stringHelper;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IConfiguration configuration,
            IStringHelper stringHelper,
            IRefreshTokenService refreshTokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepo = userRepository;
            _rolesRepo = rolesRepository;
            _configuration = configuration;
            _stringHelper = stringHelper;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }


        public async Task<LoginResultDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            loginRequestDto.Username = loginRequestDto.Username.Trim();

            if (string.IsNullOrWhiteSpace(loginRequestDto.Username)
                || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                return new LoginResultDto
                {
                    AccessToken = null,
                    RefreshToken = null,
                    Status = LoginResultDto.LoginResultStatus.InvalidCredentials,
                    Message = "Invalid Credentials."
                };
            }

            var userResult = await _userRepo.FindByUsernameAsync(loginRequestDto.Username);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                return new LoginResultDto
                {
                    AccessToken = null,
                    RefreshToken = null,
                    Status = LoginResultDto.LoginResultStatus.InvalidCredentials,
                    Message = "Invalid Credentials."
                };
            }


            if (!_stringHelper.Verify(loginRequestDto.Password, userResult.Data.PasswordHash))
            {
                return new LoginResultDto
                {
                    AccessToken = null,
                    RefreshToken = null,
                    Status = LoginResultDto.LoginResultStatus.InvalidCredentials,
                    Message = "Invalid Credentials."
                };
            }


            var hasValidRefreshTokenResult = await _refreshTokenRepository.HasValidRefreshTokenAsync(userResult.Data.UserId);
            hasValidRefreshTokenResult.ThrowIfNotSuccess();

            if (hasValidRefreshTokenResult.Data)
            {
                return new LoginResultDto
                {
                    AccessToken = null,
                    RefreshToken = null,
                    Status = LoginResultDto.LoginResultStatus.AlreadyLoggedIn,
                    Message = "Already Logged In."
                };
            }

            var accessToken = await GenerateAccessToken(userResult.Data);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenByUserIdAsync(userResult.Data.UserId);

            return new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Status = LoginResultDto.LoginResultStatus.Success,
                Message = "Login Success."
            };
        }

        public async Task<AuthResponseDto?> RefreshAsync(RefreshTokenRequestDto refreshDto)
        {
            if (!ValidateRefreshRequest(refreshDto))
                return null; // Return null to indicate refresh failure due to invalid request without throwing an exception


            var validRefreshTokenId = await GetValidRefreshTokenId(refreshDto);
            if (validRefreshTokenId == null)
                return null; // Return null to indicate refresh failure due to no valid refresh token without throwing an exception


            // Revoke the old refresh token
            if (!await RevokeRefreshToken(validRefreshTokenId.Value))
            {
                return null; // Return null to indicate refresh failure due to token revocation failure without throwing an exception
            }

            try
            {
                // Generate new access token and refresh token
                var accessToken = await GenerateAccessToken(refreshDto.Username);
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenByUsernameAsync(refreshDto.Username);

                if (accessToken is not null && refreshToken is not null)
                {
                    return new AuthResponseDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    };
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public async Task LogoutAsync(LogoutRequestDto logoutDto)
        {
            if (string.IsNullOrEmpty(logoutDto.RefreshToken)
                || string.IsNullOrEmpty(logoutDto.Username))
                return; // No need to throw an error for missing parameters during logout

            // Find the valid refresh token for the user
            var tokenResult =
                await _refreshTokenRepository.FindValidTokensByUsername(logoutDto.Username);

            if (!tokenResult.IsSuccess || tokenResult.Data == null || tokenResult.Data.Count == 0)
            {
                return; // No need to throw an error for no valid refresh token during logout
            }

            foreach (var token in tokenResult.Data)
            {
                if (_stringHelper.Verify(logoutDto.RefreshToken, token.TokenHash))
                {
                    await _refreshTokenRepository.RevokeAsync(token.RefreshTokenId);
                    break; // Exit the loop after revoking the matching token
                }
            }
        }



        private async Task<string?> GenerateAccessToken(string username)
        {
            var user = await _userRepo.FindByUsernameAsync(username);

            if (user == null || user.Data == null)
            {
                return null;
            }

            var role = await _rolesRepo.FindRoleNameByIdAsync(user.Data.RoleId);

            if (role == null || string.IsNullOrWhiteSpace(role.Data))
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Data.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Data.Username),
                new Claim(ClaimTypes.Role, role.Data),
                new Claim("RoleId", user.Data.RoleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["SMS_JWT_SECRET_KEY"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Constants.AccessTokenPeriod),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private async Task<string> GenerateAccessToken(User user)
        {
            var role = await _rolesRepo.FindRoleNameByIdAsync(user.RoleId);

            if (role == null || string.IsNullOrWhiteSpace(role.Data))
            {
                throw new InvalidOperationException("User role not found.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role.Data),
                new Claim("RoleId", user.RoleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["SMS_JWT_SECRET_KEY"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Constants.AccessTokenPeriod),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private bool ValidateRefreshRequest(RefreshTokenRequestDto refreshDto)
        {
            refreshDto.Username = refreshDto.Username.Trim();

            if (string.IsNullOrWhiteSpace(refreshDto.RefreshToken)
                || string.IsNullOrWhiteSpace(refreshDto.Username))
            {
                return false; // Return null to indicate refresh failure without throwing an exception
            }

            return true;
        }

        private async Task<Guid?> GetValidRefreshTokenId(RefreshTokenRequestDto refreshDto)
        {
            var tokensResult =
            await _refreshTokenRepository.FindValidTokensByUsername(refreshDto.Username);

            if (!tokensResult.IsSuccess || tokensResult.Data == null || !tokensResult.Data.Any())
            {
                return null;
            }

            foreach (var token in tokensResult.Data)
            {
                if (_stringHelper.Verify(refreshDto.RefreshToken, token.TokenHash))
                {
                    return token.RefreshTokenId;
                }
            }

            return null;
        }

        private async Task<bool> RevokeRefreshToken(Guid refreshTokenId)
        {
            var revokeResult = await _refreshTokenRepository.RevokeAsync(refreshTokenId);
            return revokeResult.IsSuccess;
        }
    }
}