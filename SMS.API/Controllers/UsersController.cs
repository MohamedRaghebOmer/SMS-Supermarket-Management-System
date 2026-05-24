using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Users;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Security.Claims;

namespace SMS.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleEntityPermissionService _role;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(IUserService userService,
            IRoleEntityPermissionService role,
            IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _role = role;
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(PermissionAction.Create, SystemEntity.Users)]
        [AuditActionType(AuditActionType.Register)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            int currentRoleId = await _role.GetRoleIdByUserIdAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"));

            if (await IsModifyingTheSameOrHigherRoleByRoleId(createUserDto.RoleId))
            {
                return Forbid("You cannot assign a role higher than your own.");
            }

            var userId = await _userService.RegisterAsync(createUserDto);
            return CreatedAtAction(nameof(GetById), new { id = userId }, new { userId });
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(id))
            {
                return Forbid("You cannot access a user with a higher role than your own.");
            }

            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }


        [HttpGet("username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(await _userService.GetUserIdByUsernameAsync(username)))
            {
                return Forbid("You cannot access a user with a higher role than your own.");
            }

            var user = await _userService.GetByUsernameAsync(username);
            return Ok(user);
        }


        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(await _userService.GetUserIdByEmailAsync(email)))
            {
                return Forbid("You cannot access a user with a higher role than your own.");
            }

            var user = await _userService.GetByEmailAsync(email);
            return Ok(user);
        }



        [HttpGet("person/{personId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> GetByPersonId([FromRoute] int personId)
        {
            if (personId <= 0)
            {
                return BadRequest("Invalid person ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(await _userService.GetUserIdByPersonIdAsync(personId)))
            {
                return Forbid("You cannot access a user with a higher role than your own.");
            }

            var user = await _userService.GetByPersonIdAsync(personId);
            return Ok(user);
        }



        [HttpGet("exists/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
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
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
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
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
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
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
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
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> GetByRoleId([FromRoute] int roleId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByRoleId((roleId)))
            {
                return Forbid("You cannot access users with a higher role than your own.");
            }

            var result = await _userService.GetByRoleIdAsync(roleId, paginationRequest);
            return Ok(result);
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _userService.GetPagedAsync(paginationRequest);
            return Ok(result);
        }



        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        [Authorize(Roles = "admin")]
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

            if (!User.Identity.IsAuthenticated
                || (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") != userId
                && !User.IsInRole("admin")))
            {
                return Forbid("You can only change your own password.");
            }

            var result = await _userService.ChangePasswordAsync(userId, dto);
            return Ok(result);
        }



        [HttpPatch("{userId:int}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> Activate([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(userId))
            {
                return Forbid("You cannot modify a user with a higher role than your own.");
            }

            var result = await _userService.ActivateAsync(userId);
            return Ok(result);
        }



        [HttpPatch("{userId:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> Deactivate([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(userId))
            {
                return Forbid("You cannot modify a user with a higher role than your own.");
            }

            var result = await _userService.DeactivateAsync(userId);
            return Ok(result);
        }



        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(PermissionAction.Read, SystemEntity.Users)]
        public async Task<IActionResult> Update([FromRoute] int userId,
            [FromBody] UpdateUserDto updateUserDto)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            if (await IsModifyingTheSameOrHigherRoleByUserId(userId))
            {
                return Forbid("You cannot modify a user with a higher role than your own.");
            }

            var result = await _userService.UpdateAsync(userId, updateUserDto);
            return Ok(result);
        }



        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid User ID.");
            }

            var targetRoleId = await _role.GetRoleIdByUserIdAsync(userId);
            var currentRole = User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            // Managers can only delete users with a lower role than themselves (i.e., regular users),
            // but not other managers or admins.
            if (currentRole.Equals("manager", StringComparison.OrdinalIgnoreCase)
                && targetRoleId <= await _role.GetRoleIdByUserIdAsync(
                    int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0")))
            {
                return Forbid("You cannot delete a user with a higher or equal role than your own.");
            }


            var result = await _userService.DeleteAsync(userId);
            return Ok(result);
        }



        private async Task<bool> IsModifyingTheSameOrHigherRoleByUserId(int affectedUserId)
        {
            string currentUserRole = User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            if (currentUserRole.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return false; // Admin can modify any user
            }

            string? currentUserIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(currentUserIdClaim, out int currentUserId) || currentUserId <= 0)
            {
                return true;
            }

            int currentRoleId = await _role.GetRoleIdByUserIdAsync(currentUserId);
            int affectedRoleId = await _role.GetRoleIdByUserIdAsync(affectedUserId);

            return affectedRoleId <= currentRoleId;
        }

        private async Task<bool> IsModifyingTheSameOrHigherRoleByRoleId(int affectedRoleId)
        {
            string currentUserRole = User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            if (currentUserRole.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return false; // Admin can modify any user
            }

            string? currentUserIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(currentUserIdClaim, out int currentUserId) || currentUserId <= 0)
            {
                return true;
            }

            int currentRoleId = await _role.GetRoleIdByUserIdAsync(currentUserId);

            return affectedRoleId <= currentRoleId;
        }

    }
}