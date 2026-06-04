using SMS.Shared.Enums;
using SMS.Shared.Guards;


namespace SMS.Domain.Entities
{
    public class CustomerLedger
    {
        private int _ledgerId;
        private int _customerId;
        private DateTime _entryDate;
        private int _createdBy;
        private int? _referenceId;
        private decimal _debitAmount;
        private decimal _creditAmount;
        private string? _notes;

        public int LedgerId
        {
            get => _ledgerId;
            private set
            {
                NumericGuard.AgainstInvalidId(value);
                _ledgerId = value;
            }
        }

        public int CustomerId
        {
            get => _customerId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _customerId = value;
            }
        }

        public DateTime EntryDate
        {
            get => _entryDate;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(EntryDate));
                _entryDate = value;
            }
        }

        public CustomerLedgerEntryType EntryType { get; set; }

        public CustomerLedgerReferenceType ReferenceType { get; set; }

        public int? ReferenceId
        {
            get => _referenceId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _referenceId = value;
            }
        }

        public decimal DebitAmount
        {
            get => _debitAmount;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(DebitAmount));
                if (value > 0 && CreditAmount > 0)
                {
                    throw new ArgumentException("Both debit amount and credit amount can not be greater that 0");
                }


                _debitAmount = value;
            }
        }

        public decimal CreditAmount
        {
            get => _creditAmount;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(CreditAmount));
                if (value > 0 && DebitAmount > 0)
                {
                    throw new ArgumentException("Both debit amount and credit amount can not be greater that 0");
                }

                _creditAmount = value;
            }
        }

        public decimal BalanceBefore { get; set; }

        public decimal BalanceAfter { get; set; }

        public int CreatedBy
        {
            get => _createdBy;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _createdBy = value;
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

        public CustomerLedger()
        {
        }

        public CustomerLedger(int customerId, DateTime entryDate, CustomerLedgerEntryType entryType,
            CustomerLedgerReferenceType referenceType, int? referenceId, decimal debitAmount,
            decimal creditAmount, decimal balanceBefore, decimal balanceAfter, int createdBy, string? notes)
        {
            CustomerId = customerId;
            EntryDate = entryDate;
            EntryType = entryType;
            ReferenceType = referenceType;
            ReferenceId = referenceId;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            CreatedBy = createdBy;
            Notes = notes;
        }

        public CustomerLedger(int ledgerId, int customerId, DateTime entryDate, CustomerLedgerEntryType entryType,
            CustomerLedgerReferenceType referenceType, int? referenceId, decimal debitAmount,
            decimal creditAmount, decimal balanceBefore, decimal balanceAfter, int createdBy, string? notes)
            : this(customerId, entryDate, entryType, referenceType, referenceId, debitAmount, creditAmount,
                balanceBefore, balanceAfter, createdBy, notes)
        {
            LedgerId = ledgerId;
        }
    }
}