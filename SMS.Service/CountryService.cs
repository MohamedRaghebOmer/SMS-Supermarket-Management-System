using SMS.Core;
using SMS.Core.DTOs;
using SMS.Core.Enums;
using SMS.Repository;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SMS.Service
{
    public class CountryService : IService<Country>
    {
        private readonly CountryRepository _countryRepository;
        private readonly Helper _helper;

        public async Task<DBResponse<int>> AddAsync(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country), "Country cannot be null.");
            }

            if (country.Mode != EntityMode.AddNew)
            {
                throw new ArgumentException(nameof(country), "Country already exists.");
            }

            if (string.IsNullOrWhiteSpace(country.CountryName))
            {
                throw new ArgumentException(nameof(country), "Country name cannot be empty.");
            }

            country.CountryName = country.CountryName.Trim();

            DBResponse<int> result = new DBResponse<int>();

            try
            {
                result = await _countryRepository.AddAsync(country);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error while adding new country.");
            }

            return result;
        }

        public async Task<DBResponse<Country>> FindAsync(int countryId)
        {
            if (countryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid country id.");
            }

            var result = new DBResponse<Country>();

            try
            {
                result = await _countryRepository.GetAsync(countryId);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while finding a country by ID.");
            }

            return result;
        }

        public async Task<DBResponse<Country>> FindAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                throw new ArgumentException(nameof(countryName), "Country name cannot be empty.");
            }

            var result = new DBResponse<Country>();

            try
            {
                result = await _countryRepository.GetByNameAsync(countryName.Trim());
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while finding a country by name.");
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            var result = new DBResponse<DataTable>();

            try
            {
                result = await _countryRepository.GetAllAsync();
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while retrieving all countries.");
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
            }

            if (lastCountryId != null && lastCountryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lastCountryId), "Last country id must be positive.");
            }

            var result = new DBResponse<DataTable>();

            try
            {
                result = await _countryRepository.GetPagedAsync(pageSize, lastCountryId);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while retrieving paged countries.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> UpdateAsync(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (country.Mode != EntityMode.Update)
            {
                throw new ArgumentException(nameof(country), "Country does not exist.");
            }

            if (string.IsNullOrWhiteSpace(country.CountryName))
            {
                throw new ArgumentException(nameof(country), "Country name cannot be empty.");
            }

            country.CountryName = country.CountryName.Trim();

            var result = new DBResponse<bool>();

            try
            {
                result = await _countryRepository.UpdateAsync(country);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while updating a country.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> DeleteAsync(int countryId)
        {
            if (countryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid Country Id.");
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _countryRepository.DeleteAsync(countryId);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while deleting a country.");
            }

            return result;
        }


        public CountryService()
        {
            _countryRepository = new CountryRepository();
            _helper = new Helper();
        }
    }
}
