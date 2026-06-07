using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public PersonRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }


        public async Task<OperationResult<int>> AddAsync(Person person)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_Insert");

            AddPersonParameters(cmd, person);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Person?>> FindByIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetById");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToPerson);
        }

        public async Task<OperationResult<Person?>> FindByNationalNoAsync(string nationalNo)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetByNationalNo");

            cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToPerson);
        }

        public async Task<OperationResult<PaginationResponse<Person>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToPerson);
        }

        public async Task<OperationResult<PaginationResponse<Person>>> GetByGenderAsync(
            Gender gender, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetPagedByGender");

            cmd.Parameters.Add("@Gender", SqlDbType.TinyInt).Value = (byte)gender;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToPerson);
        }

        public async Task<OperationResult<Person?>> FindByEmailAsync(string email)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetByEmail");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToPerson);
        }

        public async Task<OperationResult<Guid?>> GetImageAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetImageGuid");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            (SqlParameter code, SqlParameter message) = await _executor.PrepareCommandAsync(cmd, conn);
            object? result = await cmd.ExecuteScalarAsync();

            return _executor.CreateOperationResult<Guid?>(result as Guid?, code, message);
        }

        public async Task<OperationResult<PaginationResponse<Person>>> GetByNationalityCountryIdAsync(
            int countryId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_GetPagedByNationalityCountryId");

            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToPerson);
        }

        public async Task<OperationResult<bool>> ExistsByIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_ExistsById");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> ExistsByNationalNoAsync(string nationalNo)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_ExistsByNationalNo");

            cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> ExistsByEmailAsync(string email)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_ExistsByEmail");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> SetImageAsync(int personId, Guid? newImageGuid)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_SetImage");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            cmd.Parameters.Add("@NewImageGuid", SqlDbType.UniqueIdentifier).Value =
                newImageGuid ?? (object)DBNull.Value;

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Person person)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_Update");

            AddPersonParameters(cmd, person, includePersonId: true);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_DeleteById");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeleteAsync(string nationalNo)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_People_DeleteByNationalNo");

            cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }


        private static Person MapToPerson(SqlDataReader reader)
        {
            var thirdNameOrdinal = reader.GetOrdinal("ThirdName");
            string? thirdName = reader.IsDBNull(thirdNameOrdinal) ? null : reader.GetString(thirdNameOrdinal);

            var emailOrdinal = reader.GetOrdinal("Email");
            string? email = reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal);

            var imageGuidOrdinal = reader.GetOrdinal("ImageGuid");
            Guid? imageGuid = reader.IsDBNull(imageGuidOrdinal) ? null : reader.GetGuid(imageGuidOrdinal);

            return new Person(
                personId: reader.GetInt32(reader.GetOrdinal("PersonId")),
                nationalNo: reader.GetString(reader.GetOrdinal("NationalNo")),
                firstName: reader.GetString(reader.GetOrdinal("FirstName")),
                secondName: reader.GetString(reader.GetOrdinal("SecondName")),
                thirdName: thirdName,
                lastName: reader.GetString(reader.GetOrdinal("LastName")),
                dateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                gender: (Gender)reader.GetByte(reader.GetOrdinal("Gender")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                phone: reader.GetString(reader.GetOrdinal("Phone")),
                email: email,
                nationalityCountryId: reader.GetInt32(reader.GetOrdinal("NationalityCountryId")),
                imageGuid: imageGuid,
                createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")));
        }

        private static void AddPersonParameters(SqlCommand cmd, Person person, bool includePersonId = false)
        {
            if (includePersonId)
            {
                cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = person.PersonId;
            }

            cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = person.FirstName;
            cmd.Parameters.Add("@SecondName", SqlDbType.NVarChar, 50).Value = person.SecondName;
            cmd.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 50).Value = person.ThirdName ?? (object)DBNull.Value;
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = person.LastName;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = person.DateOfBirth.Date;
            cmd.Parameters.Add("@Gender", SqlDbType.TinyInt).Value = (byte)person.Gender;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 200).Value = person.Address;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = person.Email ?? (object)DBNull.Value;
            cmd.Parameters.Add("@NationalityCountryId", SqlDbType.Int).Value = person.NationalityCountryId;
            cmd.Parameters.Add("@ImageGuid", SqlDbType.UniqueIdentifier).Value =
                    person.ImageGuid ?? (object)DBNull.Value;
        }
    }
}
