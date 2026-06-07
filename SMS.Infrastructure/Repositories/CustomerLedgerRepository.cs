using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class CustomerLedgerRepository : ICustomerLedgerRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public CustomerLedgerRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(CustomerLedger ledger)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_Insert");

            AddCustomerLedgerParameters(cmd, ledger);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<CustomerLedger?>> FindByIdAsync(int ledgerId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetById");

            cmd.Parameters.Add("@LedgerId", SqlDbType.Int).Value = ledgerId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCustomerLedger);
        }

        public async Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomerLedger);
        }

        public async Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByCustomerIdAsync(
            int customerId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetPagedByCustomerId");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomerLedger);
        }

        public async Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByEntryTypeAsync(
            Shared.Enums.CustomerLedgerEntryType entryType, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetPagedByEntryType");

            cmd.Parameters.Add("@EntryType", SqlDbType.TinyInt).Value = (byte)entryType;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomerLedger);
        }

        public async Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByReferenceTypeAsync(
            Shared.Enums.CustomerLedgerReferenceType referenceType, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetPagedByReferenceType");

            cmd.Parameters.Add("@ReferenceType", SqlDbType.TinyInt).Value = (byte)referenceType;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomerLedger);
        }

        public async Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByCreatedByAsync(
            int createdBy, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_GetPagedByCreatedBy");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = createdBy;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToCustomerLedger);
        }

        public async Task<OperationResult<bool>> ExistsByIdAsync(int ledgerId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_CustomerLedger_ExistsById");

            cmd.Parameters.Add("@LedgerId", SqlDbType.Int).Value = ledgerId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        private static CustomerLedger MapToCustomerLedger(SqlDataReader reader)
        {
            var referenceIdOrdinal = reader.GetOrdinal("ReferenceId");
            int? referenceId = reader.IsDBNull(referenceIdOrdinal)
                ? null
                : reader.GetInt32(referenceIdOrdinal);

            var notesOrdinal = reader.GetOrdinal("Notes");
            string? notes = reader.IsDBNull(notesOrdinal) ? null : reader.GetString(notesOrdinal);

            return new CustomerLedger(
                ledgerId: reader.GetInt32(reader.GetOrdinal("LedgerId")),
                customerId: reader.GetInt32(reader.GetOrdinal("CustomerId")),
                entryDate: reader.GetDateTime(reader.GetOrdinal("EntryDate")),
                entryType: (Shared.Enums.CustomerLedgerEntryType)reader.GetByte(reader.GetOrdinal("EntryType")),
                referenceType: (Shared.Enums.CustomerLedgerReferenceType)reader.GetByte(reader.GetOrdinal("ReferenceType")),
                referenceId: referenceId,
                debitAmount: reader.GetDecimal(reader.GetOrdinal("DebitAmount")),
                creditAmount: reader.GetDecimal(reader.GetOrdinal("CreditAmount")),
                balanceBefore: reader.GetDecimal(reader.GetOrdinal("BalanceBefore")),
                balanceAfter: reader.GetDecimal(reader.GetOrdinal("BalanceAfter")),
                createdBy: reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                notes: notes);
        }

        private static void AddCustomerLedgerParameters(SqlCommand cmd, CustomerLedger ledger)
        {
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = ledger.CustomerId;
            cmd.Parameters.Add("@EntryType", SqlDbType.TinyInt).Value = (byte)ledger.EntryType;
            cmd.Parameters.Add("@ReferenceType", SqlDbType.TinyInt).Value =
                (byte)ledger.ReferenceType;
            cmd.Parameters.Add("@ReferenceId", SqlDbType.Int).Value =
                ledger.ReferenceId ?? (object)DBNull.Value;
            cmd.Parameters.Add("@DebitAmount", SqlDbType.Decimal).Value = ledger.DebitAmount;
            cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = ledger.CreditAmount;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 250).Value =
                ledger.Notes ?? (object)DBNull.Value;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = ledger.CreatedBy;
        }
    }
}
