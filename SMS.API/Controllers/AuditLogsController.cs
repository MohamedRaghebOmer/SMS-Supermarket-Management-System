using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Net;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/audit-logs")]
    [RequirePermission(PermissionAction.Read, SystemEntity.AuditLogs)]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _service;

        public AuditLogsController(IAuditLogService service)
        {
            _service = service;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("id/{id:int}", Name = "GetAuditLogById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var auditLog = await _service.GetAsync(id);
            return Ok(auditLog);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("correlation/{correlationId:guid}", Name = "GetAuditLogByCorrelationId")]
        public async Task<IActionResult> GetByCorrelationId([FromRoute] Guid correlationId)
        {
            var auditLog = await _service.GetByCorrelationIdAsync(correlationId);
            return Ok(auditLog);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedAsync(paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("user/{userId:int}", Name = "GetAuditLogsByUserId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByUserId([FromRoute] int userId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByUserIdAsync(userId, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("action/{action}", Name = "GetAuditLogByActionType")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByActionType([FromRoute] AuditActionType action,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByActionTypeAsync(action, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("endpoint/{endpoint}", Name = "GetAuditLogByEndpoint")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByEndpoint([FromRoute] string endpoint,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByEndpointAsync(endpoint, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("http-code/{httpCode}", Name = "GetAuditLogByHttpCode")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByHttpCode([FromRoute] HttpStatusCode httpCode,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByHttpStatusCodeAsync(httpCode, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("ip-address/{ipAddress}", Name = "GetAuditLogByIpAddress")]
        public async Task<IActionResult> GetByIpAddress([FromRoute] string ipAddress,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByIpAddressAsync(ipAddress, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("before-date/{date:datetime}", Name = "GetAuditLogBeforeDate")]
        public async Task<IActionResult> GetBeforeDate([FromRoute] DateTime date,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedCreatedBeforeAsync(date, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("after-date/{date:datetime}", Name = "GetAuditLogAfterDate")]
        public async Task<IActionResult> GetAfterDate([FromRoute] DateTime date,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedCreatedAfterAsync(date, paginationRequest);
            return Ok(auditLogs);
        }
    }
}
