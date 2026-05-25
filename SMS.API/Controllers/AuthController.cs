using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Auth;
using SMS.Contracts.Responses.Auth;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        [AuditActionType(AuditActionType.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResult = await _authService.LoginAsync(request);

            if (loginResult.Status == LoginResultDto.LoginResultStatus.InvalidCredentials)
            {
                return Unauthorized(loginResult.Message);
            }

            if (loginResult.Status == LoginResultDto.LoginResultStatus.AlreadyLoggedIn)
            {
                return Ok(loginResult.Message);
            }

            return Ok(new AuthResponseDto
            {
                AccessToken = loginResult.AccessToken,
                RefreshToken = loginResult.RefreshToken
            });
        }



        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        [AuditActionType(AuditActionType.TokenRefresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var authResponse = await _authService.RefreshAsync(request);
            return authResponse is null ? Unauthorized("Invalid Refresh Token or Username") : Ok(authResponse);
        }



        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [AuditActionType(AuditActionType.Logout)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await _authService.LogoutAsync(request);
            return NoContent();
        }
    }
}
