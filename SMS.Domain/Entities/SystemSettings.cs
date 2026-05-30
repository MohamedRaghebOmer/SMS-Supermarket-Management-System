using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public class SystemSettings
    {
        private decimal _maxCreditLimit;
        private decimal _minimumPaymentPercent;
        private int _graceDays;
        private int _feesFrequencyDays;
        private decimal _feesPercent;
        private decimal _capPercent;
        private DateTime _updatedAt;


        public decimal MaxCreditLimit
        {
            get => _maxCreditLimit;

            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(MaxCreditLimit));
                _maxCreditLimit = value;
            }
        }

        public decimal MinimumPaymentPercent
        {
            get => _minimumPaymentPercent;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(MinimumPaymentPercent));
                _minimumPaymentPercent = value;
            }
        }

        public int GraceDays
        {
            get => _graceDays;

            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(GraceDays));
                _graceDays = value;
            }
        }

        public int FeesFrequencyDays
        {
            get => _feesFrequencyDays;

            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(FeesFrequencyDays));
                _feesFrequencyDays = value;
            }
        }

        public decimal FeesPercent
        {
            get => _feesPercent;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(FeesPercent));
                _feesPercent = value;
            }
        }

        public decimal GracePercent
        {
            get => _capPercent;

            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(GracePercent));
                _capPercent = value;
            }
        }

        public bool AllowCreditSales { get; set; }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(UpdatedAt));
                _updatedAt = value;
            }
        }


        public SystemSettings() { }

        public SystemSettings(decimal maxCreditLimit, decimal minimumPaymentPercent, int graceDays, int feesFrequencyDays, decimal feesPercent, decimal capPercent, bool allowCreditSales, DateTime updatedAt)
        {
            MaxCreditLimit = maxCreditLimit;
            MinimumPaymentPercent = minimumPaymentPercent;
            GraceDays = graceDays;
            FeesFrequencyDays = feesFrequencyDays;
            FeesPercent = feesPercent;
            GracePercent = capPercent;
            AllowCreditSales = allowCreditSales;
            UpdatedAt = updatedAt;
        }
    }
}
