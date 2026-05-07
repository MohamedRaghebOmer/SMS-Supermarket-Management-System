using SMS.Application.Common.Guards;
using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;

namespace SMS.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repo;

        public CountryService(ICountryRepository repo)
        {
            _repo = repo;
        }


        public async Task<int> AddAsync(CreateCountryRequestDto dto)
        {
            Guard.AgainstNullOrEmptyString(dto.CountryName, "Country name");

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            Guard.AgainstInvalidId(id);

            var result = await _repo.ExistsAsync(id);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> ExistsAsync(string countryName)
        {
            Guard.AgainstNullOrEmptyString(countryName, "Country name");

            var result = await _repo.ExistsAsync(countryName);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<CountryResponseDto> FindAsync(int id)
        {
            Guard.AgainstInvalidId(id);

            var result = await _repo.GetAsync(id);
            result.ThrowIfNotSuccess();

            if (result.Data is null)
            {
                throw new NotFoundException($"Country with Id {id} was not found.");
            }

            return result.Data.ToResponseDto();
        }

        public async Task<CountryResponseDto> FindAsync(string countryName)
        {
            Guard.AgainstNullOrEmptyString(countryName, "Country name");

            var result = await _repo.GetByNameAsync(countryName);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToResponseDto();
        }

        public async Task<IReadOnlyList<CountryResponseDto>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            result.ThrowIfNotSuccess();
            return result.Data.Select(c => c.ToResponseDto()).ToList();
        }

        public async Task<IReadOnlyList<CountryResponseDto>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            Guard.AgainstInvalidId(pageSize);

            if (lastCountryId is not null)
            {
                Guard.AgainstInvalidId(lastCountryId.Value);
            }

            var result = await _repo.GetPagedAsync(pageSize, lastCountryId);
            result.ThrowIfNotSuccess();

            return result.Data.Select(c => c.ToResponseDto()).ToList();
        }

        public async Task<bool> UpdateAsync(int countryId, UpdateCountryRequestDto dto)
        {
            Guard.AgainstInvalidId(countryId);
            Guard.AgainstNullOrEmptyString(dto.CountryName, "Country name");

            var result = await _repo.UpdateAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Guard.AgainstInvalidId(id);
            var result = await _repo.DeleteAsync(id);
            result.ThrowIfNotSuccess();
            return result.Data;
        }
    }
}
