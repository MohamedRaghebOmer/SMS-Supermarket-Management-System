using Microsoft.AspNetCore.Http;
using SMS.Contracts.Requests.People;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Services
{
    public interface IPersonService
    {
        Task<int> AddAsync(CreatePersonRequestDto dto, IFormFile? image);
        Task<PersonResponseDto> GetByIdAsync(int personId);
        Task<PersonResponseDto> GetByNationalNoAsync(string nationalNo);
        Task<PaginationResponse<PersonResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<PaginationResponse<PersonResponseDto>> GetByGenderAsync(Gender gender, PaginationRequest paginationRequest);
        Task<PersonResponseDto> GetByEmailAsync(string email);
        Task<PaginationResponse<PersonResponseDto>> GetByNationalityCountryIdAsync(int countryId, PaginationRequest paginationRequest);
        Task<FileResponse> GetImageAsync(int personId);
        Task<bool> ExistsByIdAsync(int personId);
        Task<bool> ExistsByNationalNoAsync(string nationalNo);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> SetImageAsync(int personId, IFormFile newImage);
        Task<bool> RemoveImageAsync(int personId);
        Task<bool> UpdateAsync(int personId, UpdatePersonRequestDto dto, IFormFile? newImage);
        Task<bool> DeleteAsync(int personId);
        Task<bool> DeleteAsync(string nationalNo);
    }
}
