using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Auth;

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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var authResponse = await _authService.LoginAsync(request);

            if (authResponse == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(authResponse);
        }



        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var authResponse = await _authService.RefreshAsync(request);

            if (authResponse == null)
            {
                // Return 200 OK with an empty body to indicate the token is invalid or expired,
                // without revealing too much information to potential attackers.
                return Ok();
            }

            return Ok(authResponse);
        }



        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await _authService.LogoutAsync(request);
            return NoContent();
        }
    }
}
