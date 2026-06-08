using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.ReturnItems;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/return-items")]
    [ApiController]
    [Authorize]
    public class ReturnItemsController : ControllerBase
    {
        private readonly IReturnItemService _service;

        public ReturnItemsController(IReturnItemService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
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

        [RequirePermission(PermissionAction.Create, SystemEntity.SaleItems)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateReturnItemRequestDto dto)
        {
            var id = await _service.AddAsync(dto);
            return CreatedAtRoute("GetReturnItemById", new { id }, id);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("id/{id:int}", Name = "GetReturnItemById")]
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
        [HttpGet("return/{returnId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByReturnId([FromRoute] int returnId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByReturnIdAsync(returnId, request);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.SaleItems)]
        [HttpGet("product/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByProductId([FromRoute] int productId, [FromQuery] PaginationRequest request)
        {
            var result = await _service.GetPagedByProductIdAsync(productId, request);
            return Ok(result);
        }
    }
}
