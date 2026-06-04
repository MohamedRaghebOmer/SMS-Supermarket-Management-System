using SMS.Shared.Enums;
using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class Sale
    {
        /*
                        (Sales)
            SaleId (PK, int, not null)
            CustomerId (FK, int, null)
            CashierId (FK, int, not null)
            PaymentMethod (tinyint, null) <Enum> => (Cash=1, CreditCard=2, DebitCard=3, MobileWallet=4, BankTransfer=5, StoreCredit=6)
            SubTotal (decimal(18,2), not null)
            DiscountAmount (decimal(18,2), not null)
            NetTotal (decimal(18,2), not null)
            PaidAmount (decimal(18,2), not null)
            ChangeAmount (decimal(18,2), not null)
            IsCredit (bit, not null)
            Notes (nvarchar(250), null)
            SaleDate (datetime2(7), not null)
         */

        private int _saleId;
        private int? _customerId;
        private int _cashierId;
        private decimal _subTotal;
        private decimal _discountAmount;
        private decimal _netTotal;
        private decimal _paidAmount;
        private decimal _changeAmount;
        private string? _notes;
        private DateTime _saleDate;

        public int SaleId
        {
            get => _saleId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _saleId = value;
            }
        }

        public DateTime SaleDate
        {
            get => _saleDate;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(SaleDate));
                _saleDate = value;
            }
        }

        public int? CustomerId
        {
            get => _customerId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _customerId = value;
            }
        }

        public int CashierId
        {
            get => _cashierId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _cashierId = value;
            }
        }

        public PaymentMethod? PaymentMethod { get; set; }

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(SubTotal));
                _subTotal = value;
            }
        }

        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(DiscountAmount));
                _discountAmount = value;
            }
        }

        public decimal NetTotal
        {
            get => _netTotal;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(NetTotal));
                _netTotal = value;
            }
        }

        public decimal PaidAmount
        {
            get => _paidAmount;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(PaidAmount));
                _paidAmount = value;
            }
        }

        public decimal ChangeAmount
        {
            get => _changeAmount;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(ChangeAmount));
                _changeAmount = value;
            }
        }

        public bool IsCredit { get; set; }

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

        public Sale()
        {
        }

        public Sale(DateTime saleDate, int? customerId, int cashierId, PaymentMethod? paymentMethod,
            decimal subTotal, decimal discountAmount, decimal netTotal, decimal paidAmount,
            decimal changeAmount, bool isCredit, string? notes)
        {
            SaleDate = saleDate;
            CustomerId = customerId;
            CashierId = cashierId;
            PaymentMethod = paymentMethod;
            SubTotal = subTotal;
            DiscountAmount = discountAmount;
            NetTotal = netTotal;
            PaidAmount = paidAmount;
            ChangeAmount = changeAmount;
            IsCredit = isCredit;
            Notes = notes;
        }

        public Sale(int saleId, DateTime saleDate, int? customerId, int cashierId, PaymentMethod? paymentMethod,
            decimal subTotal, decimal discountAmount, decimal netTotal, decimal paidAmount, decimal changeAmount,
            bool isCredit, string? notes)
            : this(saleDate, customerId, cashierId, paymentMethod, subTotal, discountAmount, netTotal, paidAmount,
                changeAmount, isCredit, notes)
        {
            SaleId = saleId;
        }
    }
}