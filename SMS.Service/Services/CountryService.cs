using SMS.Core;
using SMS.Core.DTOs;
using SMS.Core.DTOs.Enums;
using System;
using System.Data;
using System.Threading.Tasks;
using SMS.Core.Interfaces;
using SMS.Repository;
using SMS.Core.Logging;

namespace SMS.Service
{
    public class CountryService : IService<Country>
    {
        private readonly IRepository<Country> _repo;
        private readonly Helper _helper = new Helper();

        public async Task<DBResponse<int>> AddAsync(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException("Country cannot be null.", nameof(country));
            }

            if (country.Mode != EntityMode.AddNew)
            {
                throw new ArgumentException("Country already exists.", nameof(country));
            }

            if (string.IsNullOrWhiteSpace(country.CountryName))
            {
                throw new ArgumentException("Country name cannot be empty.", nameof(country.CountryName));
            }

            country.CountryName = country.CountryName.Trim();

            DBResponse<int> result = new DBResponse<int>();

            try
            {
                result = await _repo.AddAsync(country);

                // Log to windows event log if there was an error during the add operation in the database level
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                // Log to database if there was an exception during the add operation in the program level
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<Country>> FindAsync(int countryId)
        {
            if (countryId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid country id.", nameof(countryId));
            }

            var result = new DBResponse<Country>();

            try
            {
                result = await _repo.GetAsync(countryId);
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<Country>> FindAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                throw new ArgumentException("Country name cannot be empty.", nameof(countryName));
            }

            var result = new DBResponse<Country>();

            try
            {
                CountryRepository countryRepository = new CountryRepository();
                result = await countryRepository.GetByNameAsync(countryName.Trim());
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            var result = new DBResponse<DataTable>();

            try
            {
                result = await _repo.GetAllAsync();
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than zero.", nameof(pageSize));
            }

            if (lastCountryId != null && lastCountryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lastCountryId), "Last country id must be positive.");
            }

            var result = new DBResponse<DataTable>();

            try
            {
                var countryRepository = new CountryRepository();
                result = await countryRepository.GetPagedAsync(pageSize, lastCountryId);
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid country id", nameof(id));
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _repo.ExistsAsync(id);
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<bool>> UpdateAsync(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException("Country cannot be null.", nameof(country));
            }

            if (country.Mode != EntityMode.Update)
            {
                throw new ArgumentException("Country does not exist.", nameof(country));
            }

            if (string.IsNullOrWhiteSpace(country.CountryName))
            {
                throw new ArgumentException("Country name cannot be empty.", nameof(country.CountryName));
            }

            country.CountryName = country.CountryName.Trim();

            var result = new DBResponse<bool>();

            try
            {
                result = await _repo.UpdateAsync(country);
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<bool>> DeleteAsync(int countryId)
        {
            if (countryId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid Country Id.", nameof(countryId));
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _repo.DeleteAsync(countryId);
                await _helper.HandelError(result, nameof(CountryService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(CountryService), new LogRepository());
            }

            return result;
        }


        public CountryService(IRepository<Country> countryRepository)
        {
            _repo = countryRepository;
        }
    }
}