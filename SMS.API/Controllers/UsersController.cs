using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.Helpers;
using SMS.API.Helpers.Constants;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Users;
using SMS.Shared.Common;
using SMS.Shared.Constants;

namespace SMS.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(IUserService userService,
            IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var userId = await _userService.RegisterAsync(createUserDto);
            return CreatedAtAction(nameof(GetById), new { id = userId }, new { userId });
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (!await IsAuthorizedAsync(id))
            {
                return Forbid();
            }

            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }


        [HttpGet("username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            var user = await _userService.GetByUsernameAsync(username);

            if (!await IsAuthorizedAsync(user.UserId))
            {
                return Forbid();
            }

            return Ok(user);
        }


        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }

            var user = await _userService.GetByEmailAsync(email);
            if (!await IsAuthorizedAsync(user.UserId))
            {
                return Forbid();
            }

            return Ok(user);
        }



        [HttpGet("person/{personId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByPersonId([FromRoute] int personId)
        {
            if (personId <= 0)
            {
                return BadRequest("Invalid person ID.");
            }

            var user = await _userService.GetByPersonIdAsync(personId);

            if (!await IsAuthorizedAsync(user.UserId))
            {
                return Forbid();
            }

            return Ok(user);
        }



        [HttpGet("exists/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExistsById([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _userService.ExistsByIdAsync(userId);
            return Ok(result);
        }


        [HttpGet("exists/username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExistsByUsername([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            var result = await _userService.ExistsByUsernameAsync(username);
            return Ok(result);
        }


        [HttpGet("exists/email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExistsByEmail([FromRoute] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }
            var result = await _userService.ExistsByEmailAsync(email);
            return Ok(result);
        }



        [HttpGet("email/{email}/owned-by/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IsEmailOwnedByUserAsync(
            [FromRoute] string email, [FromRoute] int userId)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }

            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _userService.IsEmailOwnedByUserAsync(email, userId);
            return Ok(result);
        }


        [HttpGet("role-id/{roleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByRoleId([FromRoute] int roleId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }

            var result = await _userService.GetByRoleIdAsync(roleId, paginationRequest);
            return Ok(result);
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _userService.GetPagedAsync(paginationRequest);
            return Ok(result);
        }



        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActiveUsers(
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _userService.GetActiveUsersAsync(paginationRequest);
            return Ok(result);
        }



        [HttpPatch("{userId:int}/change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword([FromRoute] int userId,
            [FromBody] ChangePasswordDto dto)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (!await IsAuthorizedAsync(userId))
            {
                return Forbid();
            }

            var result = await _userService.ChangePasswordAsync(userId, dto);
            return Ok(result);
        }



        [HttpPatch("{userId:int}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _userService.ActivateAsync(userId);
            return Ok(result);
        }



        [HttpPatch("{userId:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _userService.DeactivateAsync(userId);
            return Ok(result);
        }



        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int userId,
            [FromBody] UpdateUserDto updateUserDto)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _userService.UpdateAsync(userId, updateUserDto);
            return Ok(result);
        }



        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid User ID.");
            }

            var result = await _userService.DeleteAsync(userId);
            return Ok(result);
        }



        private async Task<bool> IsAuthorizedAsync(int userId)
        {
            var result = await _authorizationService.AuthorizeAsync(
                User, userId, PoliciesNames.UserOwnerOrAdmin);

            return result.Succeeded;
        }
    }
}