using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Common;
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
        [HttpGet("{auditLogId}", Name = "GetAuditLogById")]
        public async Task<IActionResult> GetById(int auditLogId)
        {
            var auditLog = await _service.GetAsync(auditLogId);
            return Ok(auditLog);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("correlation/{correlationId}", Name = "GetAuditLogByCorrelationId")]
        public async Task<IActionResult> GetByCorrelationId(Guid correlationId)
        {
            var auditLog = await _service.GetByCorrelationIdAsync(correlationId);
            return Ok(auditLog);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("all", Name = "GetAllAuditLogs")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedAsync(paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("user/{userId}", Name = "GetAuditLogByUserId")]
        public async Task<IActionResult> GetByUserId(int userId, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByUserIdAsync(userId, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("action/{action}", Name = "GetAuditLogByActionType")]
        public async Task<IActionResult> GetByActionType(AuditActionType action, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByActionTypeAsync(action, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("endpoint/{endpoint}", Name = "GetAuditLogByEndpoint")]
        public async Task<IActionResult> GetByEndpoint(string endpoint, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByEndpointAsync(endpoint, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("http-code/{httpCode}", Name = "GetAuditLogByHttpCode")]
        public async Task<IActionResult> GetByHttpCode(HttpStatusCode httpCode, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByHttpStatusCodeAsync(httpCode, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ip-address/{ipAddress}", Name = "GetAuditLogByIpAddress")]
        public async Task<IActionResult> GetByIpAddress(string ipAddress, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedByIpAddressAsync(ipAddress, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("before-date/{date}", Name = "GetAuditLogBeforeDate")]
        public async Task<IActionResult> GetBeforeDate(DateTime date, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedCreatedBeforeAsync(date, paginationRequest);
            return Ok(auditLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("after-date/{date}", Name = "GetAuditLogAfterDate")]
        public async Task<IActionResult> GetAfterDate(DateTime date, [FromQuery] PaginationRequest paginationRequest)
        {
            var auditLogs = await _service.GetPagedCreatedAfterAsync(date, paginationRequest);
            return Ok(auditLogs);
        }
    }
}
