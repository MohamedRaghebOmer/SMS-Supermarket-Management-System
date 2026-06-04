using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class Customer
    {
        private int _customerId;
        private int _personId;
        private DateTime _joinDate;
        private byte _paymentDay;
        private DateTime? _lastPaymentDate;
        private DateTime? _nextDueDate;
        private string? _notes;

        public int CustomerId
        {
            get => _customerId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _customerId = value;
            }
        }

        public int PersonId
        {
            get => _personId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _personId = value;
            }
        }

        public DateTime JoinDate
        {
            get => _joinDate;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(JoinDate));
                _joinDate = value.Date;
            }
        }

        public bool IsActive { get; set; }

        public byte PaymentDay
        {
            get => _paymentDay;
            set
            {
                if (value is < 1 or > 31)
                {
                    throw new ArgumentOutOfRangeException(nameof(PaymentDay), "PaymentDay must be between 1 and 31.");
                }

                _paymentDay = value;
            }
        }

        public decimal CurrentBalance { get; set; }

        public DateTime? LastPaymentDate
        {
            get => _lastPaymentDate;
            set
            {
                if (value.HasValue)
                {
                    DateGuard.AgainstFutureDate(value.Value, nameof(LastPaymentDate));
                }

                _lastPaymentDate = value;
            }
        }

        public DateTime? NextDueDate
        {
            get => _nextDueDate;
            set
            {
                if (value.HasValue)
                {
                    DateGuard.AgainstPastDate(value.Value, nameof(NextDueDate));
                }

                _nextDueDate = value?.Date;
            }
        }

        public string? Notes
        {
            get => _notes;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _notes = null;
                }
                else
                {
                    StringGuard.EnsureLengthInRange(value, 1, 250, nameof(Notes));
                    _notes = value.Trim();
                }
            }
        }

        public Customer()
        {
        }

        public Customer(int personId, DateTime joinDate, bool isActive, byte paymentDay,
            decimal currentBalance, DateTime? lastPaymentDate, DateTime? nextDueDate, string? notes)
        {
            PersonId = personId;
            JoinDate = joinDate;
            IsActive = isActive;
            PaymentDay = paymentDay;
            CurrentBalance = currentBalance;
            LastPaymentDate = lastPaymentDate;
            NextDueDate = nextDueDate;
            Notes = notes;
        }

        public Customer(int customerId, int personId, DateTime joinDate, bool isActive,
            byte paymentDay, decimal currentBalance, DateTime? lastPaymentDate, DateTime? nextDueDate, string? notes)
            : this(personId, joinDate, isActive, paymentDay, currentBalance, lastPaymentDate, nextDueDate, notes)
        {
            CustomerId = customerId;
        }
    }
}