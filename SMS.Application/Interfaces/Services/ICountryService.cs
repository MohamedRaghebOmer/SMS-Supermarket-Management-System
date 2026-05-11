using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface ICountryService
    {
        public Task<int> AddAsync(CreateCountryRequestDto dto);
        public Task<bool> ExistsAsync(int id);
        public Task<bool> ExistsAsync(string countryName);
        public Task<CountryResponseDto> GetAsync(int id);
        public Task<CountryResponseDto> GetAsync(string countryName);
        public Task<PaginationResponse<CountryResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);
        public Task<bool> UpdateAsync(int countryId, UpdateCountryRequestDto dto);
        public Task<bool> DeleteAsync(int id);
    }
}
