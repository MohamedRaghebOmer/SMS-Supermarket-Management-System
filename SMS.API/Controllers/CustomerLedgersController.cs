using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.CustomerLedgers;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Security.Claims;

namespace SMS.API.Controllers
{
    [Route("api/customer-ledgers")]
    [ApiController]
    [Authorize]
    public class CustomerLedgersController : ControllerBase
    {
        private readonly ICustomerLedgerService _service;

        public CustomerLedgersController(ICustomerLedgerService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.CustomerLedger)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateCustomerLedgerRequestDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.AddAsync(dto, userId);
            return CreatedAtRoute("GetCustomerLedgerById", new { ledgerId = result }, result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("exists/{ledgerId:int}", Name = "DoesCustomerLedgerExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ExistsById([FromRoute] int ledgerId)
        {
            var result = await _service.ExistsByIdAsync(ledgerId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("id/{ledgerId:int}", Name = "GetCustomerLedgerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int ledgerId)
        {
            var result = await _service.GetByIdAsync(ledgerId);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("customer/{customerId:int}", Name = "GetCustomerLedgersByCustomerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCustomerId([FromRoute] int customerId, [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("entry-type/{entryType}", Name = "GetCustomerLedgersByEntryType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByEntryType([FromRoute] CustomerLedgerEntryType entryType,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByEntryTypeAsync(entryType, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("reference-type/{referenceType}", Name = "GetCustomerLedgersByReferenceType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByReferenceType([FromRoute] CustomerLedgerReferenceType referenceType,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByReferenceTypeAsync(referenceType, paginationRequest);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.CustomerLedger)]
        [HttpGet("created-by/{userId:int}", Name = "GetCustomerLedgersByCreatedBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByCreatedBy([FromRoute] int userId, [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedByCreatedByAsync(userId, paginationRequest);
            return Ok(result);
        }

    }
}
