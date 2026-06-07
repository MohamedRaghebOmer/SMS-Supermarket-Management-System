namespace SMS.Domain.Entities
{
    public sealed class ProductStock
    {
        private int _productId;
        private decimal _quantityOnHand;
        private decimal _reorderLevel;
        private DateTime _updatedAt;

        public int ProductId
        {
            get => _productId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _productId = value;
            }
        }

        public decimal QuantityOnHand
        {
            get => _quantityOnHand;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(QuantityOnHand));
                _quantityOnHand = value;
            }
        }

        public decimal ReorderLevel
        {
            get => _reorderLevel;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstNegativeNumber(value, nameof(ReorderLevel));
                _reorderLevel = value;
            }
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                SMS.Shared.Guards.DateGuard.AgainstFutureDate(value, nameof(UpdatedAt));
                _updatedAt = value;
            }
        }

        public ProductStock()
        {
        }

        public ProductStock(int productId, decimal quantityOnHand, decimal reorderLevel, DateTime updatedAt)
        {
            ProductId = productId;
            QuantityOnHand = quantityOnHand;
            ReorderLevel = reorderLevel;
            UpdatedAt = updatedAt;
        }
    }
}
