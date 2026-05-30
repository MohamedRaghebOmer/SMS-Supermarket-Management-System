using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.SystemSettings;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/system-settings")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _service;

        public SystemSettingsController(ISystemSettingsService service)
        {
            _service = service;
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Get()
        {
            var result = await _service.GetSystemSettingsAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromBody] UpdateSystemSettingsRequestDto dto)
        {
            var result = await _service.UpdateSystemSettingsAsync(dto);
            return result ? Ok("System settings updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("max-credit-limit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetMaxCreditLimit()
        {
            var result = await _service.GetMaxCreditLimitAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("max-credit-limit/{maxCreditLimit:decimal}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateMaxCreditLimit([FromRoute] decimal maxCreditLimit)
        {
            var result = await _service.UpdateMaxCreditLimitAsync(maxCreditLimit);
            return result ? Ok("Max credit limit updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("minimum-payment-percent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetMinimumPaymentPercent()
        {
            var result = await _service.GetMinimumPaymentPercentAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("minimum-payment-percent/{minimumPaymentPercent:decimal}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateMinimumPaymentPercent([FromRoute] decimal minimumPaymentPercent)
        {
            var result = await _service.UpdateMinimumPaymentPercentAsync(minimumPaymentPercent);
            return result ? Ok("Minimum payment percent updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("grace-days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetGraceDays()
        {
            var result = await _service.GetGraceDaysAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("grace-days/{graceDays:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateGraceDays([FromRoute] int graceDays)
        {
            var result = await _service.UpdateGraceDaysAsync(graceDays);
            return result ? Ok("Grace days updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("fees-frequency-days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetFeesFrequencyDays()
        {
            var result = await _service.GetFeesFrequencyDaysAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("fees-frequency-days/{feesFrequencyDays:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateFeesFrequencyDays([FromRoute] int feesFrequencyDays)
        {
            var result = await _service.UpdateFeesFrequencyDaysAsync(feesFrequencyDays);
            return result ? Ok("Fees frequency days updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("fees-percent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetFeesPercent()
        {
            var result = await _service.GetFeesPercentAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("fees-percent/{feesPercent:decimal}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateFeesPercent([FromRoute] decimal feesPercent)
        {
            var result = await _service.UpdateFeesPercentAsync(feesPercent);
            return result ? Ok("Fees percent updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("cap-percent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCapPercent()
        {
            var result = await _service.GetCapPercentAsync();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("cap-percent/{capPercent:decimal}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateCapPercent([FromRoute] decimal capPercent)
        {
            var result = await _service.UpdateCapPercentAsync(capPercent);
            return result ? Ok("Cap percent updated successfully.") : NotFound("System settings not found.");
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.SystemSettings)]
        [HttpGet("allow-credit-sales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetAllowCreditSales()
        {
            var result = await _service.IsCreditSalesAllowed();
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.SystemSettings)]
        [HttpPut("allow-credit-sales/{allowCreditSales:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateAllowCreditSales([FromRoute] bool allowCreditSales)
        {
            var result = await _service.UpdateAllowCreditSalesAsync(allowCreditSales);
            return result ? Ok("Allow credit sales updated successfully.") : NotFound("System settings not found.");
        }
    }
}
