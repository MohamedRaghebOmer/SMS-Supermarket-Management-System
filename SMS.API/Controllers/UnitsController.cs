using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Units;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/units")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _service;

        public UnitsController(IUnitService service)
        {
            _service = service;
        }

        [RequirePermission(PermissionAction.Create, SystemEntity.Units)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] CreateUnitRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtRoute("GetUnitById", new { id = result }, result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Units)]
        [HttpGet("id/{id:int}", Name = "GetUnitById")]
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

        [RequirePermission(PermissionAction.Read, SystemEntity.Units)]
        [HttpGet("name/{unitName}", Name = "GetUnitByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByName([FromRoute] string unitName)
        {
            var result = await _service.GetByNameAsync(unitName);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Units)]
        [HttpGet("symbol/{symbol}", Name = "GetUnitBySymbol")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetBySymbol([FromRoute] string symbol)
        {
            var result = await _service.GetBySymbolAsync(symbol);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Read, SystemEntity.Units)]
        [HttpGet("is-decimal", Name = "GetUnitsByIsDecimal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetByIsDecimal([FromQuery] PaginationRequest paginationRequest, [FromQuery] bool isDecimal)
        {
            var result = await _service.GetPagedByIsDecimalAsync(paginationRequest, isDecimal);
            return Ok(result);
        }

        [RequirePermission(PermissionAction.Update, SystemEntity.Units)]
        [HttpPut("{unitId:int}", Name = "UpdateUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromRoute] int unitId, [FromBody] UpdateUnitRequestDto dto)
        {
            var result = await _service.UpdateAsync(unitId, dto);
            return result ? Ok("Unit updated successfully.") : NotFound("Unit not found.");
        }
    }
}
