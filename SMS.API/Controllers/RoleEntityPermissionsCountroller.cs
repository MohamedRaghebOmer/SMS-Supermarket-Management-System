using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.RoleEntityPermissions;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/role-entity-permissions")]
    [ApiController]
    public class RoleEntityPermissionsController : ControllerBase
    {
        private readonly IRoleEntityPermissionService _service;

        public RoleEntityPermissionsController(IRoleEntityPermissionService service)
        {
            this._service = service;
        }


        [RequirePermission(PermissionAction.Create, SystemEntity.RoleEntityPermissions)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create([FromBody] RoleEntityPermissionsRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return result ? Ok(result) : BadRequest(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.RoleEntityPermissions)]
        [HttpGet("role/{roleId:int}", Name = "GetByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByRoleId([FromRoute] int roleId)
        {
            var result = await _service.GetByRoleIdAsync(roleId);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.RoleEntityPermissions)]
        [HttpGet("entity/{entity}", Name = "GetByEntityId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByEntityId([FromRoute] SystemEntity entity)
        {
            var result = await _service.GetByEntityAsync(entity);
            return Ok(result);
        }




        [RequirePermission(PermissionAction.Read, SystemEntity.RoleEntityPermissions)]
        [HttpGet("mask/role/{roleId:int}/entity/{entity}", Name = "GetPermissionsMask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetPermissionsMask([FromRoute] int roleId, [FromRoute] SystemEntity entity)
        {
            var result = await _service.GetPermissionsMaskAsync(roleId, entity);
            return Ok(result);
        }




        [RequirePermission(PermissionAction.Update, SystemEntity.RoleEntityPermissions)]
        [HttpPatch("role/{roleId:int}/entity/{entity}", Name = "UpdatePermissionsMask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdatePermissionsMask([FromRoute] int roleId, [FromRoute] SystemEntity entity,
            [FromBody] int permissionsMask)
        {
            var result = await _service.UpdatePermissionsMaskAsync(roleId, entity, permissionsMask);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Delete, SystemEntity.RoleEntityPermissions)]
        [HttpDelete("role/{roleId:int}/entity/{entity}", Name = "DeletePermissionsMask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMask([FromRoute] int roleId, [FromRoute] SystemEntity entity)
        {
            var result = await _service.DeleteByRoleAndEntityAsync(roleId, entity);
            return Ok(result);
        }




        [RequirePermission(PermissionAction.Read, SystemEntity.RoleEntityPermissions)]
        [HttpGet("has-permission/role/{roleId:int}/entity/{entity}/action/{permissionAction}", Name = "HasPermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> HasPermission([FromRoute] int roleId, [FromRoute] SystemEntity entity,
            [FromRoute] PermissionAction permissionAction)
        {
            var result = await _service.HasPermissionAsync(roleId, entity, permissionAction);
            return Ok(result);
        }
    }
}
