namespace SMS.Contracts.Responses
{
    public sealed record SystemSettingsResponseDto
    {
        public decimal MaxCreditLimit { get; init; }
        public decimal MinimumPaymentPercent { get; init; }
        public int? GraceDays { get; init; }
        public int FeesFrequencyDays { get; init; }
        public decimal FeesPercent { get; init; }
        public decimal CapPercent { get; init; }
        public bool AllowCreditSales { get; init; }
    }
}
