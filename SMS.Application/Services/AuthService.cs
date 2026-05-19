using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Auth;
using SMS.Contracts.Responses.Auth;
using SMS.Domain.Entities;
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

        public AuthService(IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IConfiguration configuration,
            IStringHelper stringHelper,
            IRefreshTokenService refreshTokenService)
        {
            _userRepo = userRepository;
            _rolesRepo = rolesRepository;
            _configuration = configuration;
            _stringHelper = stringHelper;
            _refreshTokenService = refreshTokenService;
        }


        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            if (string.IsNullOrEmpty(loginRequestDto.Username)
                || string.IsNullOrEmpty(loginRequestDto.Password))
            {
                return null; // Return null to indicate login failure without throwing an exception
            }

            var userResult = await _userRepo.FindByUsernameAsync(loginRequestDto.Username);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                return null; // Return null to indicate login failure without throwing an exception
            }


            if (!_stringHelper.Verify(loginRequestDto.Password, userResult.Data.PasswordHash))
            {
                return null; // Return null to indicate login failure without throwing an exception
            }

            var accessToken = await GenerateAccessToken(userResult.Data);
            if (accessToken == null)
            {
                return null; // Return null to indicate login failure without throwing an exception
            }

            var refreshToken = await _refreshTokenService
                .GenerateRefreshTokenAsync(userResult.Data.Username);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthResponseDto?> RefreshAsync(RefreshTokenRequestDto refreshDto)
        {
            if (string.IsNullOrEmpty(refreshDto.RefreshToken)
                || string.IsNullOrEmpty(refreshDto.Username)
                || !await _refreshTokenService.IsValidRefreshTokenByUsernameAsync(
                    refreshDto.RefreshToken, refreshDto.Username))
            {
                return null; // Return null to indicate refresh failure without throwing an exception
            }

            return new AuthResponseDto
            {
                AccessToken = await GenerateAccessToken(refreshDto.Username),
                RefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(refreshDto.Username),
            };
        }

        public async Task LogoutAsync(LogoutRequestDto logoutDto)
        {
            if (string.IsNullOrEmpty(logoutDto.RefreshToken)
                || string.IsNullOrEmpty(logoutDto.Username))
                return; // No need to throw an error for missing parameters during logout

            await _refreshTokenService.RevokeRefreshTokenAsync(logoutDto.RefreshToken);
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

            var claims = new List<Claim>();
            {
                new Claim(ClaimTypes.NameIdentifier, user.Data.UserId.ToString());
                new Claim(ClaimTypes.Name, user.Data.Username);
                new Claim(ClaimTypes.Role, role.Data);
            }

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["SMS_JWT_SECRET_KEY"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private async Task<string?> GenerateAccessToken(User user)
        {
            if (user == null)
            {
                return null;
            }

            var role = await _rolesRepo.FindRoleNameByIdAsync(user.RoleId);

            if (role == null || string.IsNullOrWhiteSpace(role.Data))
            {
                return null;
            }

            var claims = new List<Claim>();
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());
                new Claim(ClaimTypes.Name, user.Username);
                new Claim(ClaimTypes.Role, role.Data);
            }

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["SMS_JWT_SECRET_KEY"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }
    }
}
