using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Common;
using SMS.Contracts.Requests.Countries;
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
        [HttpPost("add", Name = "AddNewCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddNewCountry(CreateCountryRequestDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("exists/{id}", Name = "DoesCountryExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Exists(int id)
        {
            var result = await _service.ExistsAsync(id);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("exists/{countryName}", Name = "DoesCountryExistByCountryName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Exists(string countryName)
        {
            var result = await _service.ExistsAsync(countryName);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("{id}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("{countryName}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetByName(string countryName)
        {
            var result = await _service.GetAsync(countryName);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Read, SystemEntity.Countries)]
        [HttpGet("all", Name = "GetAllCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedAsync(paginationRequest);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Update, SystemEntity.Countries)]
        [HttpPut("update/{countryId}", Name = "UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCountry(int countryId, UpdateCountryRequestDto updateCountryRequestDto)
        {
            var result = await _service.UpdateAsync(countryId, updateCountryRequestDto);
            return Ok(result);
        }



        [RequirePermission(PermissionAction.Delete, SystemEntity.Countries)]
        [HttpDelete("delete/{countryId}", Name = "DeleteCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCountryById(int countryId)
        {
            var result = await _service.DeleteAsync(countryId);
            return Ok(result);
        }
    }
}
