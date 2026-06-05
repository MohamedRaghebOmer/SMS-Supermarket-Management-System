using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Products;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.Products)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateProductRequestDto dto)
        {
            var id = await _service.AddAsync(dto);
            return CreatedAtRoute("GetProductById", new { id }, id);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("id/{id:int}", Name = "GetProductById")]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCategory([FromRoute] int categoryId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetByCategoryIdAsync(categoryId, request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("name/{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByName([FromRoute] string productName)
        {
            var result = await _service.GetByNameAsync(productName);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("sku/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetBySku([FromRoute] string sku)
        {
            var result = await _service.GetBySkuAsync(sku);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("unit/{unitId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByUnit([FromRoute] int unitId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByUnitIdAsync(unitId, request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("discount-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByDiscountRange(decimal minPercent,
            decimal maxPercent, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByDiscountRangeAsync(request, minPercent, maxPercent);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByIsActive(bool isActive,
            [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByIsActiveAsync(request, isActive);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("created-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCreatedRange([FromQuery] PaginationRequest request,
            [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetPagedByCreatedAtRangeAsync(request, from, to);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("updated-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByUpdatedRange([FromQuery] PaginationRequest request,
            [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetPagedByUpdatedAtRangeAsync(request, from, to);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("discount/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetDiscount([FromRoute] int productId)
        {
            var result = await _service.GetDiscountPercentAsync(productId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Products)]
        [HttpGet("image-guid/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetImageGuid([FromRoute] int productId)
        {
            var result = await _service.GetImageGuidAsync(productId);
            return result.HasValue? Ok(result) : Ok();
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Products)]
        [HttpPut("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromRoute] int productId, [FromBody] UpdateProductRequestDto dto)
        {
            var result = await _service.UpdateAsync(productId, dto);
            return result ? Ok("Product updated successfully.") : NotFound("Product not found.");
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Products)]
        [HttpPatch("{productId:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Deactivate([FromRoute] int productId)
        {
            var result = await _service.DeactivateAsync(productId);
            return result ? Ok("Product deactivated successfully.") : NotFound("Product not found.");
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Products)]
        [HttpPatch("{productId:int}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Activate([FromRoute] int productId)
        {
            var result = await _service.ActivateAsync(productId);
            return result ? Ok("Product activated successfully.") : NotFound("Product not found.");
        }
    }
}