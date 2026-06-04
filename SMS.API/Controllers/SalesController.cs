using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Sales;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Security.Claims;

namespace SMS.API.Controllers
{
    [Route("api/sales")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.Sales)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateSaleRequestDto dto)
        {
            int cashierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.AddAsync(dto, cashierId);
            return CreatedAtRoute("GetSaleById", new { saleId = result }, result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
        [HttpGet("exists/{saleId:int}", Name = "DoesSaleExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ExistsById([FromRoute] int saleId)
        {
            var result = await _service.ExistsByIdAsync(saleId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
        [HttpGet("id/{saleId:int}", Name = "GetSaleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int saleId)
        {
            var result = await _service.GetByIdAsync(saleId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
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


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
        [HttpGet("cashier/{cashierId:int}", Name = "GetSalesByCashierId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCashierId([FromRoute] int cashierId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByCashierIdAsync(cashierId, paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
        [HttpGet("customer/{customerId:int}", Name = "GetSalesByCustomerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCustomerId([FromRoute] int customerId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Sales)]
        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByDateRangeAsync(startDate, endDate, paginationRequest);
            return Ok(result);
        }
    }
}