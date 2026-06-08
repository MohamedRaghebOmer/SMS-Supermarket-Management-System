namespace SMS.Domain.Entities
{
    public sealed class Return
    {

        /*
                     (Returns)

           ReturnId (PK, int, not null)
           SaleId (FK, int, not null)
           CustomerId (FK, int, null)
           ReturnReason (nvarchar(250), null)
           ReturnTotal (decimal(18,2), not null)
           CreatedBy (FK, int, not null)
           ReturnDate (datetime2(7), not null)
         */

        private int _returnId;
        private int _saleId;
        private int? _customerId;
        private string? _returnReason;
        private decimal _returnTotal;
        private int _createdBy;
        private DateTime _returnDate;

        public int ReturnId
        {
            get => _returnId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _returnId = value;
            }
        }

        public int SaleId
        {
            get => _saleId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _saleId = value;
            }
        }

        public int? CustomerId
        {
            get => _customerId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _customerId = value;
            }
        }

        public string? ReturnReason
        {
            get => _returnReason;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _returnReason = null;
                }
                else
                {
                    SMS.Shared.Guards.StringGuard.EnsureLengthInRange(value, 1, 250, nameof(ReturnReason));
                    _returnReason = value.Trim();
                }
            }
        }

        public decimal ReturnTotal
        {
            get => _returnTotal;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(ReturnTotal));
                _returnTotal = value;
            }
        }

        public int CreatedBy
        {
            get => _createdBy;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _createdBy = value;
            }
        }

        public DateTime ReturnDate
        {
            get => _returnDate;
            set
            {
                SMS.Shared.Guards.DateGuard.AgainstFutureDate(value, nameof(ReturnDate));
                _returnDate = value;
            }
        }

        public Return()
        {
        }

        public Return(int saleId, int? customerId, string? returnReason, decimal returnTotal, int createdBy)
        {
            SaleId = saleId;
            CustomerId = customerId;
            ReturnReason = returnReason;
            ReturnTotal = returnTotal;
            CreatedBy = createdBy;
        }

        public Return(int returnId, int saleId, int? customerId, string? returnReason, decimal returnTotal,
            int createdBy, DateTime returnDate)
            : this(saleId, customerId, returnReason, returnTotal, createdBy)
        {
            ReturnId = returnId;
            ReturnDate = returnDate;
        }
    }
}
