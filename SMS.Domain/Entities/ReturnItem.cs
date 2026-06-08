namespace SMS.Domain.Entities
{
    public sealed class ReturnItem
    {

        /*
         
                        (Return Items)

           ReturnItemId (PK, int, not null)
           ReturnId (FK, int, not null)
           SaleItemId (FK, int, not null)
           ProductId (FK, int, not null)
           Quantity (decimal(18,3), not null)
           UnitPrice (not null)
           LineTotal (not null) = Quantity * UnitPrice
         
         */
        private int _returnItemId;
        private int _returnId;
        private int _saleItemId;
        private int _productId;
        private decimal _quantity;
        private decimal _unitPrice;
        private decimal _lineTotal;

        public int ReturnItemId
        {
            get => _returnItemId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _returnItemId = value;
            }
        }

        public int ReturnId
        {
            get => _returnId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _returnId = value;
            }
        }

        public int SaleItemId
        {
            get => _saleItemId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _saleItemId = value;
            }
        }

        public int ProductId
        {
            get => _productId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _productId = value;
            }
        }

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNonPositiveNumber(value, nameof(Quantity));
                _quantity = value;
            }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNonPositiveNumber(value, nameof(UnitPrice));
                _unitPrice = value;
            }
        }

        public decimal LineTotal
        {
            get => _lineTotal;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNonPositiveNumber(value, nameof(LineTotal));
                _lineTotal = value;
            }
        }

        public ReturnItem()
        {
        }

        public ReturnItem(int returnId, int saleItemId, int productId, decimal quantity, decimal unitPrice, decimal lineTotal)
        {
            ReturnId = returnId;
            SaleItemId = saleItemId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            LineTotal = lineTotal;
        }

        public ReturnItem(int returnItemId, int returnId, int saleItemId, int productId, decimal quantity, decimal unitPrice, decimal lineTotal)
            : this(returnId, saleItemId, productId, quantity, unitPrice, lineTotal)
        {
            ReturnItemId = returnItemId;
        }
    }
}
