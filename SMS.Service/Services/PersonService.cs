using SMS.Core;
using SMS.Core.DTOs;
using SMS.Core.Enums;
using SMS.Repository;
using System;
using System.Data;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SMS.Service
{
    public class PersonService : IService<Person>
    {
        private readonly PersonRepository _personRepository;
        private readonly Helper _helper;

        public async Task<DBResponse<int>> AddAsync(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person can not be null.");
            }

            if (person.Mode != EntityMode.AddNew)
            {
                throw new ArgumentException("The person is already exists.");
            }

            if (string.IsNullOrWhiteSpace(person.NationalNo))
            {
                throw new ArgumentException("National No is required.", nameof(person.NationalNo));
            }

            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                throw new ArgumentException("First name is required.", nameof(person.FirstName));
            }

            if (string.IsNullOrWhiteSpace(person.SecondName))
            {
                throw new ArgumentException("Second name No is required.", nameof(person.SecondName));
            }

            if (string.IsNullOrWhiteSpace(person.LastName))
            {
                throw new ArgumentException("Last name is required.", nameof(person.LastName));
            }

            if (string.IsNullOrWhiteSpace(person.Address))
            {
                throw new ArgumentException("Address is required.", nameof(person.Address));
            }

            if (person.CountryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(person.CountryId), "Invalid country id.");
            }

            if (!Core.Helpers.ValidationHelper.IsAdult(person.DateOfBirth))
            {
                throw new ArgumentException("Person must be older than 18 years old.", nameof(person.Gender));
            }

            if (!Enum.IsDefined(typeof(Gender), person.Gender))
            {
                throw new ArgumentException("Invalid gender.", nameof(person.Gender));
            }

            if (!string.IsNullOrWhiteSpace(person.Email) && !Core.Helpers.ValidationHelper.IsValidEmail(person.Email))
            {
                throw new ArgumentException("Invalid email format.", nameof(person.Email));
            }

            var result = new DBResponse<int>();

            try
            {
                result = await _personRepository.AddAsync(person);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error occurred while adding new person.");
            }

            return result;
        }

        public async Task<DBResponse<Person>> FindAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid person id.");
            }

            var result = new DBResponse<Person>();

            try
            {
                result = await _personRepository.GetAsync(id);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error while fetching person with id: " + id);
            }

            return result;
        }

        public async Task<DBResponse<Person>> FindAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                throw new ArgumentException("National no is required.", nameof(nationalNo));
            }

            var result = new DBResponse<Person>();

            try
            {
                result = await _personRepository.GetAsync(nationalNo);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error fetching person with NationalNo: " + nationalNo);
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastId)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Invalid page size.");
            }

            if (lastId != null && lastId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid person id.", nameof(lastId));
            }

            var result = new DBResponse<DataTable>();

            try
            {
                result = await _personRepository.GetPagedAsync(pageSize, lastId);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error get paging from people.");
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            var result = new DBResponse<DataTable>();

            try
            {
                result = await _personRepository.GetAllAsync();
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while retrieving all countries.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid person id");
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _personRepository.ExistsAsync(id);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error check person exists");
            }

            return result;
        }

        public async Task<DBResponse<bool>> UpdateAsync(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person can not be null.");
            }

            if (person.Mode != EntityMode.Update)
            {
                throw new ArgumentException(nameof(person), "Person does not exist.");
            }

            if (string.IsNullOrWhiteSpace(person.NationalNo))
            {
                throw new ArgumentException("National No is required.", nameof(person.NationalNo));
            }

            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                throw new ArgumentException("First name is required.", nameof(person.FirstName));
            }

            if (string.IsNullOrWhiteSpace(person.SecondName))
            {
                throw new ArgumentException("Second name No is required.", nameof(person.SecondName));
            }

            if (string.IsNullOrWhiteSpace(person.LastName))
            {
                throw new ArgumentException("Last name is required.", nameof(person.LastName));
            }

            if (string.IsNullOrWhiteSpace(person.Address))
            {
                throw new ArgumentException("Address is required.", nameof(person.Address));
            }

            if (person.CountryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(person.CountryId), "Invalid country id.");
            }

            if (!Core.Helpers.ValidationHelper.IsAdult(person.DateOfBirth))
            {
                throw new ArgumentException("Person must be older than 18 years old.", nameof(person.Gender));
            }

            if (!Enum.IsDefined(typeof(Gender), person.Gender))
            {
                throw new ArgumentException("Invalid gender.", nameof(person.Gender));
            }

            if (!string.IsNullOrWhiteSpace(person.Email) && !Core.Helpers.ValidationHelper.IsValidEmail(person.Email))
            {
                throw new ArgumentException("Invalid email format.", nameof(person.Email));
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _personRepository.UpdateAsync(person);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while updating a person.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> DeleteAsync(int personId)
        {
            if (personId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(personId), "Invalid Person Id.");
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _personRepository.DeleteAsync(personId);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while deleting a country.");
            }

            return result;
        }


        public PersonService()
        {
            this._personRepository = new PersonRepository();
            this._helper = new Helper();
        }
    }
}
