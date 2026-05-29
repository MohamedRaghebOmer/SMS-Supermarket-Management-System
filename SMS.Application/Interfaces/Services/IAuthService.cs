using SMS.Contracts.Requests.Auth;
using SMS.Contracts.Responses.Auth;

namespace SMS.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<AuthResponseDto?> RefreshAsync(RefreshTokenRequestDto refreshDto);
        Task LogoutAsync(LogoutRequestDto logoutDto);
    }
}