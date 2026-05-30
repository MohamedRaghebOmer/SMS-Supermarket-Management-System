using SMS.Contracts.Requests.SystemSettings;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    internal static class SystemSettingsMapper
    {
        public static SystemSettings ToEntity(this UpdateSystemSettingsRequestDto dto)
        {
            return new SystemSettings(
                maxCreditLimit: dto.MaxCreditLimit,
                minimumPaymentPercent: dto.MinimumPaymentPercent,
                graceDays: dto.GraceDays,
                feesFrequencyDays: dto.FeesFrequencyDays,
                feesPercent: dto.FeesPercent,
                capPercent: dto.CapPercent,
                allowCreditSales: dto.AllowCreditSales,
                updatedAt: DateTime.UtcNow);
        }

        public static SystemSettingsResponseDto ToDto(this SystemSettings entity)
        {
            return new SystemSettingsResponseDto
            {
                MaxCreditLimit = entity.MaxCreditLimit,
                MinimumPaymentPercent = entity.MinimumPaymentPercent,
                GraceDays = entity.GraceDays,
                FeesFrequencyDays = entity.FeesFrequencyDays,
                FeesPercent = entity.FeesPercent,
                CapPercent = entity.CapPercent,
                AllowCreditSales = entity.AllowCreditSales
            };
        }
    }
}
