using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.SaleItems;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/sale-items")]
    [ApiController]
    [Authorize]
    public class SaleItemsController : ControllerBase
    {
        private readonly ISaleItemService _service;

        public SaleItemsController(ISaleItemService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
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

        [RequirePermission(PermissionAction.Create, SystemEntity.SaleItems)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateSaleItemRequestDto dto)
        {
            var id = await _service.AddAsync(dto);
            return CreatedAtRoute("GetSaleItemById", new { id }, id);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("id/{id:int}", Name = "GetSaleItemById")]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("sale/{saleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetBySaleId([FromRoute] int saleId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedBySaleIdAsync(saleId, request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("product/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByProductId([FromRoute] int productId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByProductIdAsync(productId, request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("sale-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetBySaleAndProduct([FromQuery] int saleId, [FromQuery] int productId)
        {
            var result = await _service.GetBySaleIdAndProductIdAsync(saleId, productId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("{id:int}/line-total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetLineTotal([FromRoute] int id)
        {
            var result = await _service.GetLineTotalByIdAsync(id);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.SaleItems)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromQuery] int saleItemId, [FromBody] UpdateSaleItemRequestDto dto)
        {
            var result = await _service.UpdateAsync(saleItemId, dto);
            return result ? Ok("Sale item updated successfully.") : NotFound("Sale item not found.");
        }

        [RequirePermission(PermissionAction.Delete, SystemEntity.SaleItems)]
        [HttpDelete("{saleItemId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete([FromRoute] int saleItemId)
        {
            var result = await _service.DeleteAsync(saleItemId);
            return result ? Ok("Sale item deleted successfully.") : NotFound("Sale item not found.");
        }
    }
}
