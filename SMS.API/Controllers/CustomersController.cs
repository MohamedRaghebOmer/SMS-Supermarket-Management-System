using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Customers;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }


        [RequirePermission(PermissionAction.Create, SystemEntity.Customers)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateCustomerRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtRoute("GetCustomerById", new { customerId = result }, result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("exists/id/{customerId:int}", Name = "DoesCustomerExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ExistsById([FromRoute] int customerId)
        {
            var result = await _service.ExistsByIdAsync(customerId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("exists/person/{personId:int}", Name = "DoesCustomerExistByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ExistsByPersonId([FromRoute] int personId)
        {
            var result = await _service.ExistsByPersonIdAsync(personId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("{customerId:int}/is-blocked", Name = "IsCustomerBlocked")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> IsBlocked([FromRoute] int customerId)
        {
            var result = await _service.IsBlockedAsync(customerId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("id/{customerId:int}", Name = "GetCustomerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetById([FromRoute] int customerId)
        {
            var result = await _service.GetByIdAsync(customerId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("person/{personId:int}", Name = "GetCustomerByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByPersonId([FromRoute] int personId)
        {
            var result = await _service.GetByPersonIdAsync(personId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
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


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("{customerId:int}/debit-amount", Name = "GetCustomerDebitAmount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetDebitAmount([FromRoute] int customerId)
        {
            var result = await _service.GetDebitAmountAsync(customerId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.Customers)]
        [HttpGet("active", Name = "GetActiveCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetActive([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedActiveAsync(paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.Customers)]
        [HttpPut("{customerId:int}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromRoute] int customerId,
            [FromBody] UpdateCustomerRequestDto dto)
        {
            var result = await _service.UpdateAsync(customerId, dto);
            return result ? Ok("Customer updated successfully.") : NotFound("Customer not found.");
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.Customers)]
        [HttpPatch("{customerId:int}", Name = "DeactivateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Deactivate([FromRoute] int customerId)
        {
            var result = await _service.DeactivateAsync(customerId);
            return result ? Ok("Customer deactivated successfully.") : NotFound("Customer not found.");
        }
    }
}