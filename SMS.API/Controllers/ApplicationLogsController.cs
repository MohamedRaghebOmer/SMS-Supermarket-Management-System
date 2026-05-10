using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Common;
using SMS.Shared.Enums;
using LogLevel = SMS.Shared.Enums.LogLevel;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/application-logs")]
    [RequirePermission(PermissionAction.Read, SystemEntity.ApplicationLogs)]
    public class ApplicationLogsController : ControllerBase
    {
        private readonly IApplicationLogService _service;

        public ApplicationLogsController(IApplicationLogService service)
        {
            _service = service;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetApplicationLogById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var applicationLog = await _service.GetAsync(id);
            return Ok(applicationLog);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("audit-log/{auditLogId}", Name = "GetApplicationLogsByAuditLogId")]
        public async Task<IActionResult> GetByAuditLogId([FromRoute] int auditLogId)
        {
            var applicationLogs = await _service.GetByAuditLogIdAsync(auditLogId);
            return Ok(applicationLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all", Name = "GetAllApplicationLogs")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest)
        {
            var applicationLogs = await _service.GetPagedAsync(paginationRequest);
            return Ok(applicationLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("log-level/{logLevel}", Name = "GetApplicationLogsByLogLevel")]
        public async Task<IActionResult> GetByLogLevel([FromRoute] LogLevel logLevel, [FromQuery] PaginationRequest paginationRequest)
        {
            var applicationLogs = await _service.GetPagedByLogLevelAsync(logLevel, paginationRequest);
            return Ok(applicationLogs);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("date-range/{startDate}/{endDate}", Name = "GetApplicationLogsByDateRange")]
        public async Task<IActionResult> GetByDateRange([FromRoute] DateTime startDate,
            [FromRoute] DateTime endDate, [FromQuery] PaginationRequest paginationRequest)
        {
            var applicationLogs = await _service.GetPagedByDateRangeAsync(startDate, endDate, paginationRequest);
            return Ok(applicationLogs);
        }


    }
}
