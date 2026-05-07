using SMS.Application.Common.Results;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        public Task<OperationResult<int>> AddAsync(Country country);
        public Task<OperationResult<bool>> ExistsAsync(int countryId);
        public Task<OperationResult<bool>> ExistsAsync(string countryName);
        public Task<OperationResult<Country>> GetAsync(int countryId);
        public Task<OperationResult<Country>> GetByNameAsync(string name);
        public Task<OperationResult<IReadOnlyList<Country>>> GetAllAsync();
        public Task<OperationResult<IReadOnlyList<Country>>> GetPagedAsync(int pageSize, int? lastCountryId);
        public Task<OperationResult<bool>> UpdateAsync(Country country);
        public Task<OperationResult<bool>> DeleteAsync(int countryId);
    }
}
