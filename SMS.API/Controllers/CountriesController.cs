using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.Countries;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountriesController(ICountryService service)
        {
            this._service = service;
        }



        [RequirePermission(PermissionAction.Create, SystemEntity.Countries)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create([FromBody] CreateCountryRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtRoute("GetById", new { id = result }, result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("exists/id/{id:int}", Name = "DoesCountryExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Exists([FromRoute] int id)
        {
            var result = await _service.ExistsAsync(id);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("exists/name/{countryName}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Exists([FromRoute] string countryName)
        {
            var result = await _service.ExistsAsync(countryName);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("id/{id:int}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("name/{countryName}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByName([FromRoute] string countryName)
        {
            var result = await _service.GetAsync(countryName);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetAll([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedAsync(paginationRequest);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.Countries)]
        [HttpPut("{countryId:int}", Name = "UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update([FromRoute] int countryId, UpdateCountryRequestDto updateCountryRequestDto)
        {
            var result = await _service.UpdateAsync(countryId, updateCountryRequestDto);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Delete, SystemEntity.Countries)]
        [HttpDelete("{countryId:int}", Name = "DeleteCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteById([FromRoute] int countryId)
        {
            var result = await _service.DeleteAsync(countryId);
            return Ok(result);
        }
    }
}
