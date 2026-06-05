using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Roles;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.Roles)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateRoleRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtRoute("GetRoleById", new { id = result }, result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("is-active/id/{id:int}", Name = "IsRoleActiveById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> IsActive([FromRoute] int id)
        {
            var result = await _service.IsActive(id);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("id/{id:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("name/{roleName}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByName([FromRoute] string roleName)
        {
            var result = await _service.GetByNameAsync(roleName);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetPaged([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedAsync(paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("active", Name = "GetActiveRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetActive([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByIsActiveAsync(paginationRequest, true);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("inactive", Name = "GetInactiveRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetInactive([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByIsActiveAsync(paginationRequest, false);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("{id:int}/name", Name = "GetRoleNameById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetRoleNameById([FromRoute] int id)
        {
            var result = await _service.GetRoleNameByIdAsync(id);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Roles)]
        [HttpGet("created-between", Name = "GetRolesByCreatedAtRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCreatedAtRange([FromQuery] PaginationRequest paginationRequest,
            [FromBody] DateTime from, [FromBody] DateTime to)
        {
            var result = await _service.GetPagedByCreatedAtRangeAsync(paginationRequest, from, to);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Roles)]
        [HttpPut("{roleId:int}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromRoute] int roleId, [FromBody] UpdateRoleRequestDto dto)
        {
            var result = await _service.UpdateAsync(roleId, dto);
            return result ? Ok("Role updated successfully.") : NotFound("Role not found.");
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Roles)]
        [HttpPatch("{roleId:int}/deactivate", Name = "DeactivateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Deactivate([FromRoute] int roleId)
        {
            var result = await _service.DeactivateAsync(roleId);
            return result ? Ok("Role deactivated successfully.") : NotFound("Role not found.");
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Roles)]
        [HttpPatch("{roleId:int}/activate", Name = "ActivateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Activate([FromRoute] int roleId)
        {
            var result = await _service.ActivateAsync(roleId);
            return result ? Ok("Role activated successfully.") : NotFound("Role not found.");
        }
    }
}
