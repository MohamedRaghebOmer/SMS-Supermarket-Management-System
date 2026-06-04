using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public CustomerRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }


        public async Task<OperationResult<int>> AddAsync(Customer customer)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_Insert");

            AddCustomerParameters(cmd, customer, includeCustomerId: false);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Customer?>> FindByIdAsync(int customerId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_GetById");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCustomer);
        }

        public async Task<OperationResult<Customer?>> FindByPersonIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_GetByPersonId");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCustomer);
        }

        public async Task<OperationResult<PaginationResponse<Customer>>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomer);
        }

        public async Task<OperationResult<PaginationResponse<Customer>>> GetPagedActiveAsync(
            PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_GetPagedActive");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomer);
        }

        public async Task<OperationResult<bool>> ExistsByIdAsync(int customerId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_ExistsById");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> ExistsByPersonIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_ExistsByPersonId");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<decimal>> GetDebitAmountAsync(int customerId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Customers_GetDebitAmount");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

            return await _executor.ExecuteScalarAsync<decimal>(cmd, conn);
        }

        /// <summary>
        /// Determines whether the specified customer is blocked.
        /// </summary>
        public async Task<OperationResult<bool>> IsBlocked(int customerId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_IsBlocked");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Customer customer)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_Update");

            AddCustomerParameters(cmd, customer, includeCustomerId: true);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int customerId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Customers_Delete");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }


        private static Customer MapToCustomer(SqlDataReader reader)
        {
            var lastPaymentDateOrdinal = reader.GetOrdinal("LastPaymentDate");
            DateTime? lastPaymentDate = reader.IsDBNull(lastPaymentDateOrdinal)
                ? null
                : reader.GetDateTime(lastPaymentDateOrdinal);

            var nextDueDateOrdinal = reader.GetOrdinal("NextDueDate");
            DateTime? nextDueDate = reader.IsDBNull(nextDueDateOrdinal)
                ? null
                : reader.GetDateTime(nextDueDateOrdinal);

            var notesOrdinal = reader.GetOrdinal("Notes");
            string? notes = reader.IsDBNull(notesOrdinal) ? null : reader.GetString(notesOrdinal);

            return new Customer(
                customerId: reader.GetInt32(reader.GetOrdinal("CustomerId")),
                personId: reader.GetInt32(reader.GetOrdinal("PersonId")),
                joinDate: reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                paymentDay: reader.GetByte(reader.GetOrdinal("PaymentDay")),
                currentBalance: reader.GetDecimal(reader.GetOrdinal("CurrentBalance")),
                lastPaymentDate: lastPaymentDate,
                nextDueDate: nextDueDate,
                notes: notes);
        }

        private static void AddCustomerParameters(SqlCommand cmd, Customer customer,
            bool includeCustomerId = false)
        {
            if (includeCustomerId)
                cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customer.CustomerId;

            cmd.Parameters.Add("@PaymentDay", SqlDbType.TinyInt).Value = customer.PaymentDay;
            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = customer.PersonId;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 250).Value =
                customer.Notes ?? (object)DBNull.Value;
        }
    }
}