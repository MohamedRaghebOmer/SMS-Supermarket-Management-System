using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Categories;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }


        [RequirePermission(PermissionAction.Create, SystemEntity.Categories)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateCategoryRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtRoute("GetCategoryById", new { id = result }, result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
        [HttpGet("is-active/id/{id:int}", Name = "IsActiveById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> IsActive([FromRoute] int id)
        {
            var result = await _service.IsActive(id);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
        [HttpGet("id/{id:int}", Name = "GetCategoryById")]
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


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
        [HttpGet("name/{categoryName}", Name = "GetCategoryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByName([FromRoute] string categoryName)
        {
            var result = await _service.GetByNameAsync(categoryName);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
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


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
        [HttpGet("active", Name = "GetActiveCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetActive([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByIsActiveAsync(paginationRequest, true);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Categories)]
        [HttpGet("inactive", Name = "GetInactiveCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetInactive([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByIsActiveAsync(paginationRequest, false);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.Categories)]
        [HttpPut("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequestDto dto)
        {
            var result = await _service.UpdateAsync(categoryId, dto);
            return result ? Ok("Category updated successfully.") : NotFound("Category not found.");
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.Categories)]
        [HttpPatch("{categoryId:int}/deactivate", Name = "DeactivateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Deactivate([FromRoute] int categoryId)
        {
            var result = await _service.DeactivateAsync(categoryId);
            return result ? Ok("Category deactivated successfully.") : NotFound("Category not found.");
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.Categories)]
        [HttpPatch("{categoryId:int}/activate", Name = "ActivateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Activate([FromRoute] int categoryId)
        {
            var result = await _service.ActivateAsync(categoryId);
            return result ? Ok("Category activated successfully.") : NotFound("Category not found.");
        }
    }
}