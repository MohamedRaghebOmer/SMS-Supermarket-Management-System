using SMS.Application.Common.Results;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ISystemSettingsRepository
    {
        public Task<OperationResult<SystemSettings>> GetSystemSettingsAsync();
        public Task<OperationResult<bool>> UpdateSystemSettingsAsync(SystemSettings settings);

        public Task<OperationResult<decimal>> GetMaxCreditLimitAsync();
        public Task<OperationResult<bool>> UpdateMaxCreditLimitAsync(decimal maxCreditLimit);

        public Task<OperationResult<decimal>> GetMinimumPaymentPercentAsync();
        public Task<OperationResult<bool>> UpdateMinimumPaymentPercentAsync(decimal minimumPaymentPercent);

        public Task<OperationResult<int>> GetGraceDaysAsync();
        public Task<OperationResult<bool>> UpdateGraceDaysAsync(int graceDays);

        public Task<OperationResult<int>> GetFeesFrequencyDaysAsync();
        public Task<OperationResult<bool>> UpdateFeesFrequencyDaysAsync(int feesFrequencyDays);

        public Task<OperationResult<decimal>> GetFeesPercentAsync();
        public Task<OperationResult<bool>> UpdateFeesPercentAsync(decimal feesPercent);

        public Task<OperationResult<decimal>> GetCapPercentAsync();
        public Task<OperationResult<bool>> UpdateCapPercentAsync(decimal capPercent);

        public Task<OperationResult<bool>> IsCreditSalesAllowed();
        public Task<OperationResult<bool>> UpdateAllowCreditSalesAsync(bool allowCreditSales);
    }
}
