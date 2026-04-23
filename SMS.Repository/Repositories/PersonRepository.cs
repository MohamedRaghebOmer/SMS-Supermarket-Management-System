using SMS.Core;
using SMS.Core.DTOs;
using SMS.Core.Enums;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly CountryRepository _countryRepository;

        public async Task<DBResponse<int>> AddAsync(Person person)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_Create"))
            {
                // Input Parameters
                cmd.Parameters.Add(new SqlParameter("@NationalNo", SqlDbType.NVarChar, 20)).Value = person.NationalNo;
                cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50)).Value = person.FirstName;
                cmd.Parameters.Add(new SqlParameter("@SecondName", SqlDbType.NVarChar, 50)).Value = person.SecondName;
                cmd.Parameters.Add(new SqlParameter("@ThirdName", SqlDbType.NVarChar, 50)).Value =
                    string.IsNullOrWhiteSpace(person.ThirdName) ? (object)DBNull.Value : person.ThirdName;
                cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50)).Value = person.LastName;
                cmd.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date)).Value = person.DateOfBirth;
                cmd.Parameters.Add(new SqlParameter("@Gender", SqlDbType.TinyInt)).Value = (byte)person.Gender;
                cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 200)).Value = person.Address;
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20)).Value = person.Phone;
                cmd.Parameters.Add(new SqlParameter("@NationalityCountryId", SqlDbType.Int)).Value = person.CountryId;
                cmd.Parameters.Add(new SqlParameter("@ImageGuid", SqlDbType.UniqueIdentifier)).Value =
                    person.ImageGuid == null ? (object)DBNull.Value : person.ImageGuid;

                // Output Parameters
                SqlParameter newId = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse<int>((int)newId.Value, code, message);
            }
        }

        public async Task<DBResponse<Person>> GetAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_GetById"))
            {
                // Input parameters
                cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Person person = new Person();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        person.Mode = EntityMode.Update;
                        person.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        person.SecondName = reader.GetString(reader.GetOrdinal("SecondName"));
                        person.ThirdName = reader.GetString(reader.GetOrdinal("ThirdName"));
                        person.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        person.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"));
                        person.Gender = (Gender)reader.GetByte(reader.GetOrdinal("Gender"));
                        person.Address = reader.GetString(reader.GetOrdinal("Address"));
                        person.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        person.CountryId = reader.GetInt32(reader.GetOrdinal("NationalityCountryId"));
                        person.Country = (await _countryRepository.GetAsync(person.CountryId)).Data;
                        person.ImageGuid = reader.GetGuid(reader.GetOrdinal("ImageGuid"));
                        person.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                        person.Email = reader.GetString(reader.GetOrdinal("Email"));
                    }
                }

                return Helper.CreateDBResponse(person, code, message);
            }
        }

        public async Task<DBResponse<Person>> GetAsync(string nationalNo)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_GetByNationalNo"))
            {
                // Input parameters
                cmd.Parameters.Add("@nationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Person person = new Person();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        person.Mode = EntityMode.Update;
                        person.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        person.SecondName = reader.GetString(reader.GetOrdinal("SecondName"));
                        person.ThirdName = reader.GetString(reader.GetOrdinal("ThirdName"));
                        person.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        person.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"));
                        person.Gender = (Gender)reader.GetByte(reader.GetOrdinal("Gender"));
                        person.Address = reader.GetString(reader.GetOrdinal("Address"));
                        person.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        person.CountryId = reader.GetInt32(reader.GetOrdinal("NationalityCountryId"));
                        person.Country = (await _countryRepository.GetAsync(person.CountryId)).Data;
                        person.ImageGuid = reader.GetGuid(reader.GetOrdinal("ImageGuid"));
                        person.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                        person.Email = reader.GetString(reader.GetOrdinal("Email"));
                    }
                }

                return Helper.CreateDBResponse(person, code, message);
            }

        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_GetPaged"))
            {
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@LastPersonId", SqlDbType.Int).Value = lastId ?? (object)DBNull.Value;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);
                await conn.OpenAsync();
                return Helper.CreateDBResponse(await Helper.ExecuteDataTableAsync(cmd), code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_GetAll"))
            {
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);
                await conn.OpenAsync();
                return Helper.CreateDBResponse(await Helper.ExecuteDataTableAsync(cmd), code, message);
            }
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_ExistsById"))
            {
                cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);
                await conn.OpenAsync();
                return Helper.CreateDBResponse<bool>(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<DBResponse<bool>> ExistsAsync(string nationalNo)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_ExistsByNationalNo"))
            {
                cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);
                await conn.OpenAsync();
                return Helper.CreateDBResponse<bool>(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<DBResponse<bool>> UpdateAsync(Person person)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_Update"))
            {
                // Input Parameters
                cmd.Parameters.Add(new SqlParameter("@NationalNo", SqlDbType.NVarChar, 20)).Value = person.NationalNo;
                cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50)).Value = person.FirstName;
                cmd.Parameters.Add(new SqlParameter("@SecondName", SqlDbType.NVarChar, 50)).Value = person.SecondName;
                cmd.Parameters.Add(new SqlParameter("@ThirdName", SqlDbType.NVarChar, 50)).Value =
                    string.IsNullOrWhiteSpace(person.ThirdName) ? (object)DBNull.Value : person.ThirdName;
                cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50)).Value = person.LastName;
                cmd.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date)).Value = person.DateOfBirth;
                cmd.Parameters.Add(new SqlParameter("@Gender", SqlDbType.TinyInt)).Value = (byte)person.Gender;
                cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 200)).Value = person.Address;
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20)).Value = person.Phone;
                cmd.Parameters.Add(new SqlParameter("@NationalityCountryId", SqlDbType.Int)).Value = person.CountryId;
                cmd.Parameters.Add(new SqlParameter("@ImageGuid", SqlDbType.UniqueIdentifier)).Value =
                    person.ImageGuid == null ? (object)DBNull.Value : person.ImageGuid;

                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);
                await conn.OpenAsync();
                var result = Helper.CreateDBResponse(code, message);

                if (result.Data)
                {
                    person.Country = (await _countryRepository.GetAsync(person.CountryId)).Data;
                }

                return result;
            }
        }

        public async Task<DBResponse<bool>> DeleteAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspPeople_Delete"))
            {
                cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }


        public PersonRepository()
        {
            this._countryRepository = new CountryRepository();
        }
    }
}
