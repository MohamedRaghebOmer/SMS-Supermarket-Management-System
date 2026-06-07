using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/product-stock")]
    [ApiController]
    [Authorize]
    public class ProductStockController : ControllerBase
    {
        private readonly IProductStockService _service;

        public ProductStockController(IProductStockService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.ProductStock)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetPaged([FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.ProductStock)]
        [HttpGet("id/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int productId)
        {
            var result = await _service.GetByIdAsync(productId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.ProductStock)]
        [HttpGet("quantity/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetQuantityOnHand([FromRoute] int productId)
        {
            var result = await _service.GetQuantityOnHandAsync(productId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.ProductStock)]
        [HttpGet("reorder-level/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetReorderLevel([FromRoute] int productId)
        {
            var result = await _service.GetReorderLevelAsync(productId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.ProductStock)]
        [HttpPatch("{productId:int}/reorder-level")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateReorderLevel([FromRoute] int productId,
            [FromBody] decimal reorderLevel)
        {
            var result = await _service.UpdateReorderLevelAsync(productId, reorderLevel);
            return result ? Ok("Reorder level updated successfully.") : NotFound("Product stock not found.");
        }
    }
}
