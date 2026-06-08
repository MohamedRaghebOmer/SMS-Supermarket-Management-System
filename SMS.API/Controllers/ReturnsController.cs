using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Returns;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Security.Claims;

namespace SMS.API.Controllers
{
    [Route("api/returns")]
    [ApiController]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly IReturnService _service;

        public ReturnsController(IReturnService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.Returns)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateReturnRequestDto dto)
        {
            var createdBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.AddAsync(dto, createdBy);
            return CreatedAtRoute("GetReturnById", new { returnId = result }, result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("id/{returnId:int}", Name = "GetReturnById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int returnId)
        {
            var result = await _service.GetByIdAsync(returnId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("sale/{saleId:int}", Name = "GetReturnsBySaleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetBySaleId([FromRoute] int saleId)
        {
            var result = await _service.GetBySaleIdAsync(saleId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("customer/{customerId:int}", Name = "GetReturnsByCustomerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCustomerId([FromRoute] int customerId, [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByDateRange([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByDateRangeAsync(startDate, endDate, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("total-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByReturnTotalRange([FromQuery] decimal minTotal, [FromQuery] decimal maxTotal, [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByReturnTotalRangeAsync(minTotal, maxTotal, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Returns)]
        [HttpGet("{returnId:int}/total", Name = "GetReturnTotalById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetReturnTotalById([FromRoute] int returnId)
        {
            var result = await _service.GetReturnTotalByIdAsync(returnId);
            return Ok(result);
        }
    }
}
