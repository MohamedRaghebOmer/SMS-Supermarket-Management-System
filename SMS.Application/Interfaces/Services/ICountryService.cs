using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;

namespace SMS.Application.Interfaces.Services
{
    public interface ICountryService
    {
        public Task<int> AddAsync(CreateCountryRequestDto dto);
        public Task<bool> ExistsAsync(int id);
        public Task<bool> ExistsAsync(string countryName);
        public Task<CountryResponseDto> FindAsync(int id);
        public Task<CountryResponseDto> FindAsync(string countryName);
        public Task<IReadOnlyList<CountryResponseDto>> GetAllAsync();
        public Task<IReadOnlyList<CountryResponseDto>> GetPagedAsync(int pageSize, int? lastCountryId);
        public Task<bool> UpdateAsync(int countryId, UpdateCountryRequestDto dto);
        public Task<bool> DeleteAsync(int id);
    }
}
