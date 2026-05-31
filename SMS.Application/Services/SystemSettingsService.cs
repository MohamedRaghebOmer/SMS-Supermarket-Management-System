using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.SystemSettings;
using SMS.Contracts.Responses;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly ISystemSettingsRepository _repo;

        public SystemSettingsService(ISystemSettingsRepository repo)
        {
            _repo = repo;
        }

        public async Task<SystemSettingsResponseDto> GetSystemSettingsAsync()
        {
            var result = await _repo.GetSystemSettingsAsync();
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<bool> UpdateSystemSettingsAsync(UpdateSystemSettingsRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateDto(dto);

            var result = await _repo.UpdateSystemSettingsAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<decimal> GetMaxCreditLimitAsync()
        {
            var result = await _repo.GetMaxCreditLimitAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateMaxCreditLimitAsync(decimal maxCreditLimit)
        {
            NumericGuard.AgainstNegativeNumber(maxCreditLimit, nameof(maxCreditLimit));
            var result = await _repo.UpdateMaxCreditLimitAsync(maxCreditLimit);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<decimal> GetMinimumPaymentPercentAsync()
        {
            var result = await _repo.GetMinimumPaymentPercentAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateMinimumPaymentPercentAsync(decimal minimumPaymentPercent)
        {
            ValidatePercent(minimumPaymentPercent, nameof(minimumPaymentPercent));
            var result = await _repo.UpdateMinimumPaymentPercentAsync(minimumPaymentPercent);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<int> GetGraceDaysAsync()
        {
            var result = await _repo.GetGraceDaysAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateGraceDaysAsync(int graceDays)
        {
            ValidateGraceDays(graceDays, nameof(graceDays));
            var result = await _repo.UpdateGraceDaysAsync(graceDays);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<int> GetFeesFrequencyDaysAsync()
        {
            var result = await _repo.GetFeesFrequencyDaysAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateFeesFrequencyDaysAsync(int feesFrequencyDays)
        {
            NumericGuard.AgainstNegativeNumber(feesFrequencyDays, nameof(feesFrequencyDays));
            var result = await _repo.UpdateFeesFrequencyDaysAsync(feesFrequencyDays);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<decimal> GetFeesPercentAsync()
        {
            var result = await _repo.GetFeesPercentAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateFeesPercentAsync(decimal feesPercent)
        {
            ValidatePercent(feesPercent, nameof(feesPercent));
            var result = await _repo.UpdateFeesPercentAsync(feesPercent);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<decimal> GetCapPercentAsync()
        {
            var result = await _repo.GetCapPercentAsync();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateCapPercentAsync(decimal capPercent)
        {
            ValidatePercent(capPercent, nameof(capPercent));
            var result = await _repo.UpdateCapPercentAsync(capPercent);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> IsCreditSalesAllowed()
        {
            var result = await _repo.IsCreditSalesAllowed();
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> UpdateAllowCreditSalesAsync(bool allowCreditSales)
        {
            var result = await _repo.UpdateAllowCreditSalesAsync(allowCreditSales);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        private static void ValidateDto(UpdateSystemSettingsRequestDto dto)
        {
            NumericGuard.AgainstNegativeNumber(dto.MaxCreditLimit, nameof(dto.MaxCreditLimit));
            ValidatePercent(dto.MinimumPaymentPercent, nameof(dto.MinimumPaymentPercent));
            NumericGuard.AgainstNegativeNumber(dto.FeesFrequencyDays, nameof(dto.FeesFrequencyDays));
            ValidatePercent(dto.FeesPercent, nameof(dto.FeesPercent));
            ValidatePercent(dto.CapPercent, nameof(dto.CapPercent));
            ValidateGraceDays(dto.GraceDays, nameof(dto.GraceDays));
        }

        private static void ValidatePercent(decimal value, string parameterName)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be between 0 and 100.");
            }
        }

        private static void ValidateGraceDays(int value, string parameterName)
        {
            if (value < 0 || value > 27)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be between 0 and 27.");
            }
        }
    }
}
