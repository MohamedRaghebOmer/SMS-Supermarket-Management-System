using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.CustomAttributes;
using SMS.Application.Interfaces.Services;
using SMS.Contracts.Requests.People;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.API.Controllers
{
    [Route("api/people")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _service;

        public PeopleController(IPersonService service)
        {
            _service = service;
        }


        [RequirePermission(PermissionAction.Create, SystemEntity.People)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create([FromForm] CreatePersonRequestDto dto,
            IFormFile? image)
        {
            var result = await _service.AddAsync(dto, image);
            return CreatedAtRoute("GetPersonById", new { personId = result }, result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("exists/id/{personId:int}", Name = "DoesPersonExistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ExistsById([FromRoute] int personId)
        {
            var result = await _service.ExistsByIdAsync(personId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("exists/national-no/{nationalNo}", Name = "DoesPersonExistByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ExistsByNationalNo([FromRoute] string nationalNo)
        {
            var result = await _service.ExistsByNationalNoAsync(nationalNo);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("exists/email/{email}", Name = "DoesPersonExistByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ExistsByEmail([FromRoute] string email)
        {
            var result = await _service.ExistsByEmailAsync(email);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("id/{personId:int}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetById([FromRoute] int personId)
        {
            var result = await _service.GetByIdAsync(personId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("{personId:int}/image", Name = "GetPersonImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetImage([FromRoute] int personId)
        {
            var stream = await _service.GetImageAsync(personId);
            return File(stream, "application/octet-stream");
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("national-no/{nationalNo}", Name = "GetPersonByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByNationalNo([FromRoute] string nationalNo)
        {
            var result = await _service.GetByNationalNoAsync(nationalNo);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("email/{email}", Name = "GetPersonByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByEmail([FromRoute] string email)
        {
            var result = await _service.GetByEmailAsync(email);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("gender/{gender}", Name = "GetPeopleByGender")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByGender([FromRoute] Gender gender,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetByGenderAsync(gender, paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("nationality/{countryId:int}", Name = "GetPeopleByNationalityCountryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetByNationalityCountryId([FromRoute] int countryId,
            [FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetByNationalityCountryIdAsync(countryId, paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Read, SystemEntity.People)]
        [HttpGet("paged", Name = "GetPeoplePaged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetPaged([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _service.GetPagedAsync(paginationRequest);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.People)]
        [HttpPut("{personId:int}", Name = "UpdatePerson")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update([FromRoute] int personId,
            [FromForm] UpdatePersonRequestDto dto,
            IFormFile? newImage)
        {
            var result = await _service.UpdateAsync(personId, dto, newImage);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Update, SystemEntity.People)]
        [HttpPatch("{personId:int}/image", Name = "UpdatePersonImage")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateImage([FromRoute] int personId,
            IFormFile newImage)
        {
            var result = await _service.UpdateImageAsync(personId, newImage);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Delete, SystemEntity.People)]
        [HttpDelete("id/{personId:int}", Name = "DeletePersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteById([FromRoute] int personId)
        {
            var result = await _service.DeleteAsync(personId);
            return Ok(result);
        }


        [RequirePermission(PermissionAction.Delete, SystemEntity.People)]
        [HttpDelete("national-no/{nationalNo}", Name = "DeletePersonByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteByNationalNo([FromRoute] string nationalNo)
        {
            var result = await _service.DeleteAsync(nationalNo);
            return Ok(result);
        }
    }
}
