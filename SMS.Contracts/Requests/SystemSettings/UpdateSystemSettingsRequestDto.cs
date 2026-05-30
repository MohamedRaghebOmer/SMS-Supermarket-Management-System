using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.SystemSettings
{
    public sealed record UpdateSystemSettingsRequestDto
    {
        [Range(0, int.MaxValue)]
        public decimal MaxCreditLimit { get; init; }

        [Range(0, 100)]
        public decimal MinimumPaymentPercent { get; init; }

        [Range(0, int.MaxValue)]
        public int? GraceDays { get; init; }

        [Range(0, int.MaxValue)]
        public int FeesFrequencyDays { get; init; }

        [Range(0, 100)]
        public decimal FeesPercent { get; init; }

        [Range(0, 100)]
        public decimal GracePercent { get; init; }

        public bool AllowCreditSales { get; init; } = true;
    }
}
