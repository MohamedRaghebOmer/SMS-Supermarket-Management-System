using SMS.Contracts.Requests.SystemSettings;
using SMS.Contracts.Responses;

namespace SMS.Application.Interfaces.Services
{
    public interface ISystemSettingsService
    {
        Task<SystemSettingsResponseDto> GetSystemSettingsAsync();
        Task<bool> UpdateSystemSettingsAsync(UpdateSystemSettingsRequestDto dto);

        Task<decimal> GetMaxCreditLimitAsync();
        Task<bool> UpdateMaxCreditLimitAsync(decimal maxCreditLimit);

        Task<decimal> GetMinimumPaymentPercentAsync();
        Task<bool> UpdateMinimumPaymentPercentAsync(decimal minimumPaymentPercent);

        Task<int> GetGraceDaysAsync();
        Task<bool> UpdateGraceDaysAsync(int graceDays);

        Task<int> GetFeesFrequencyDaysAsync();
        Task<bool> UpdateFeesFrequencyDaysAsync(int feesFrequencyDays);

        Task<decimal> GetFeesPercentAsync();
        Task<bool> UpdateFeesPercentAsync(decimal feesPercent);

        Task<decimal> GetCapPercentAsync();
        Task<bool> UpdateCapPercentAsync(decimal capPercent);

        Task<bool> IsCreditSalesAllowed();
        Task<bool> UpdateAllowCreditSalesAsync(bool allowCreditSales);
    }
}
