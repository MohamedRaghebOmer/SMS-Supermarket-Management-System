using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class SystemSettingsRepository : ISystemSettingsRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public SystemSettingsRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<SystemSettings>> GetSystemSettingsAsync()
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_SystemSettings_Get");

            return (await _executor.ExecuteSingleAsync(cmd, conn, MapToSystemSettings))!;
        }

        public async Task<OperationResult<bool>> UpdateSystemSettingsAsync(SystemSettings settings)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SystemSettings_Update");

            AddSystemSettingsParameters(cmd, settings);
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<decimal>> GetMaxCreditLimitAsync()
        {
            return await _executor.ExecuteDecimalScalarAsync("usp_SystemSettings_GetMaxCreditLimit");
        }

        public async Task<OperationResult<bool>> UpdateMaxCreditLimitAsync(decimal maxCreditLimit)
        {
            return await _executor.ExecuteDecimalUpdateAsync("usp_SystemSettings_UpdateMaxCreditLimit",
                "@MaxCreditLimit", maxCreditLimit);
        }

        public async Task<OperationResult<decimal>> GetMinimumPaymentPercentAsync()
        {
            return await _executor.ExecuteDecimalScalarAsync("usp_SystemSettings_GetMinimumPaymentPercent");
        }

        public async Task<OperationResult<bool>> UpdateMinimumPaymentPercentAsync(decimal minimumPaymentPercent)
        {
            return await _executor.ExecuteDecimalUpdateAsync("usp_SystemSettings_UpdateMinimumPaymentPercent",
                "@MinimumPaymentPercent", minimumPaymentPercent);
        }

        public async Task<OperationResult<int>> GetGraceDaysAsync()
        {
            return await _executor.ExecuteIntScalarAsync("usp_SystemSettings_GetGraceDays");
        }

        public async Task<OperationResult<bool>> UpdateGraceDaysAsync(int graceDays)
        {
            return await _executor.ExecuteIntUpdateAsync("usp_SystemSettings_UpdateGraceDays", "@GraceDays", graceDays);
        }

        public async Task<OperationResult<int>> GetFeesFrequencyDaysAsync()
        {
            return await _executor.ExecuteIntScalarAsync("usp_SystemSettings_GetFeesFrequencyDays");
        }

        public async Task<OperationResult<bool>> UpdateFeesFrequencyDaysAsync(int feesFrequencyDays)
        {
            return await _executor.ExecuteIntUpdateAsync("usp_SystemSettings_UpdateFeesFrequencyDays",
                "@FeesFrequencyDays", feesFrequencyDays);
        }

        public async Task<OperationResult<decimal>> GetFeesPercentAsync()
        {
            return await _executor.ExecuteDecimalScalarAsync("usp_SystemSettings_GetFeesPercent");
        }

        public async Task<OperationResult<bool>> UpdateFeesPercentAsync(decimal feesPercent)
        {
            return await _executor.ExecuteDecimalUpdateAsync("usp_SystemSettings_UpdateFeesPercent", "@FeesPercent",
                feesPercent);
        }

        public async Task<OperationResult<decimal>> GetCapPercentAsync()
        {
            return await _executor.ExecuteDecimalScalarAsync("usp_SystemSettings_GetCapPercent");
        }

        public async Task<OperationResult<bool>> UpdateCapPercentAsync(decimal capPercent)
        {
            return await _executor.ExecuteDecimalUpdateAsync("usp_SystemSettings_UpdateCapPercent", "@CapPercent",
                capPercent);
        }

        public async Task<OperationResult<bool>> IsCreditSalesAllowed()
        {
            return await _executor.ExecuteBoolScalarAsync("usp_SystemSettings_IsCreditSalesAllowed");
        }

        public async Task<OperationResult<bool>> UpdateAllowCreditSalesAsync(bool allowCreditSales)
        {
            return await _executor.ExecuteBoolUpdateAsync("usp_SystemSettings_UpdateAllowCreditSales",
                "@AllowCreditSales", allowCreditSales);
        }

        private static SystemSettings MapToSystemSettings(SqlDataReader reader)
        {
            return new SystemSettings(
                maxCreditLimit: reader.GetDecimal(reader.GetOrdinal("MaxCreditLimit")),
                minimumPaymentPercent: reader.GetDecimal(reader.GetOrdinal("MinimumPaymentPercent")),
                graceDays: reader.GetInt32(reader.GetOrdinal("GraceDays")),
                feesFrequencyDays: reader.GetInt32(reader.GetOrdinal("FeesFrequencyDays")),
                feesPercent: reader.GetDecimal(reader.GetOrdinal("FeesPercent")),
                capPercent: reader.GetDecimal(reader.GetOrdinal("CapPercent")),
                allowCreditSales: reader.GetBoolean(reader.GetOrdinal("AllowCreditSales")),
                updatedAt: reader.GetDateTime(reader.GetOrdinal("UpdatedAt")));
        }

        private static void AddSystemSettingsParameters(SqlCommand cmd, SystemSettings settings)
        {
            cmd.Parameters.Add("@MaxCreditLimit", SqlDbType.Decimal).Value = settings.MaxCreditLimit;
            cmd.Parameters.Add("@MinimumPaymentPercent", SqlDbType.Decimal).Value = settings.MinimumPaymentPercent;
            cmd.Parameters.Add("@GraceDays", SqlDbType.Int).Value = settings.GraceDays;
            cmd.Parameters.Add("@FeesFrequencyDays", SqlDbType.Int).Value = settings.FeesFrequencyDays;
            cmd.Parameters.Add("@FeesPercent", SqlDbType.Decimal).Value = settings.FeesPercent;
            cmd.Parameters.Add("@CapPercent", SqlDbType.Decimal).Value = settings.CapPercent;
            cmd.Parameters.Add("@AllowCreditSales", SqlDbType.Bit).Value = settings.AllowCreditSales;
        }
    }
}