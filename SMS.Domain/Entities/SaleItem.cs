namespace SMS.Domain.Entities
{
    public class SaleItem
    {
        /*

                        (SaleItems)

           SaleItemId (PK, int, not null)
           SaleId (FK, int, not null)
           ProductId (FK, int, not null)
           Quantity (decimal(18,3), not null)
           UnitCostPriceAtSale (decimal(18,2), not null)
           UnitSellPriceAtSale (decimal(18,2), not null)
           DiscountAmount (decimal(18,2), not null)
           LineTotal (decimal(18,2), not null)

         */

        private int _saleItemId;
        private int _saleId;
        private int _productId;
        private decimal _quantity;
        private decimal _unitCostPriceAtSale;
        private decimal _unitSellPriceAtSale;
        private decimal _discountAmount;
        private decimal _lineTotal;

        public int SaleItemId
        {
            get => _saleItemId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _saleItemId = value;
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
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(Quantity));
                _quantity = value;
            }
        }

        public decimal UnitCostPriceAtSale
        {
            get => _unitCostPriceAtSale;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(UnitCostPriceAtSale));
                _unitCostPriceAtSale = value;
            }
        }

        public decimal UnitSellPriceAtSale
        {
            get => _unitSellPriceAtSale;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(UnitSellPriceAtSale));
                _unitSellPriceAtSale = value;
            }
        }

        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(DiscountAmount));
                _discountAmount = value;
            }
        }

        public decimal LineTotal
        {
            get => _lineTotal;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(LineTotal));
                _lineTotal = value;
            }
        }

        public SaleItem()
        {
        }

        public SaleItem(int saleId, int productId, decimal quantity, decimal unitCostPriceAtSale,
            decimal unitSellPriceAtSale, decimal discountAmount, decimal lineTotal)
        {
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
            UnitCostPriceAtSale = unitCostPriceAtSale;
            UnitSellPriceAtSale = unitSellPriceAtSale;
            DiscountAmount = discountAmount;
            LineTotal = lineTotal;
        }

        public SaleItem(int saleItemId, int saleId, int productId, decimal quantity, decimal unitCostPriceAtSale,
            decimal unitSellPriceAtSale, decimal discountAmount, decimal lineTotal)
            : this(saleId, productId, quantity, unitCostPriceAtSale, unitSellPriceAtSale, discountAmount, lineTotal)
        {
            SaleItemId = saleItemId;
        }
    }
}
