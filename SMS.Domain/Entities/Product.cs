using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class Product
    {
        private int _productId;
        private int _categoryId;
        private string _productName = null!;
        private string _sku = null!;
        private string? _description;
        private int _unitId;
        private decimal _costPrice;
        private decimal _sellPrice;
        private decimal _discountPercent;
        private DateTime _createdAt;
        private DateTime? _updatedAt;

        public int ProductId
        {
            get => _productId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _productId = value;
            }
        }

        public int CategoryId
        {
            get => _categoryId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _categoryId = value;
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(ProductName));
                StringGuard.AgainstExcessiveLength(value, 150, nameof(ProductName));
                _productName = value.Trim();
            }
        }

        public string SKU
        {
            get => _sku;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(SKU));
                StringGuard.AgainstExcessiveLength(value, 50, nameof(SKU));
                _sku = value.Trim();
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                StringGuard.AgainstExcessiveLength(value, 250, nameof(Description));
                _description = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        public int UnitId
        {
            get => _unitId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _unitId = value;
            }
        }

        public decimal CostPrice
        {
            get => _costPrice;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(CostPrice));
                _costPrice = value;
            }
        }

        public decimal SellPrice
        {
            get => _sellPrice;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(SellPrice));
                _sellPrice = value;
            }
        }

        public decimal DiscountPercent
        {
            get => _discountPercent;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(DiscountPercent),
                        "Discount percent must be between 0 and 100.");
                _discountPercent = value;
            }
        }

        public Guid? ImageGuid { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(CreatedAt));
                _createdAt = value;
            }
        }

        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (value.HasValue)
                {
                    DateGuard.AgainstFutureDate(value.Value, nameof(UpdatedAt));
                }

                _updatedAt = value;
            }
        }

        public Product()
        {
        }

        // Create constructor
        public Product(int categoryId, string productName, string sku, string? description,
            int unitId, decimal costPrice, decimal sellPrice, decimal discountPercent,
            Guid? imageGuid, DateTime createdAt)
        {
            CategoryId = categoryId;
            ProductName = productName;
            SKU = sku;
            Description = description;
            UnitId = unitId;
            CostPrice = costPrice;
            SellPrice = sellPrice;
            DiscountPercent = discountPercent;
            ImageGuid = imageGuid;
            IsActive = true;
            CreatedAt = createdAt;
        }

        // Update constructor including product id and isActive
        public Product(int productId, int categoryId, string productName, string sku, string? description,
            int unitId, decimal costPrice, decimal sellPrice, decimal discountPercent,
            Guid? imageGuid, bool isActive, DateTime createdAt, DateTime? updatedAt)
            : this(categoryId, productName, sku, description, unitId, costPrice, sellPrice, discountPercent, imageGuid,
                createdAt)
        {
            ProductId = productId;
            IsActive = isActive;
            UpdatedAt = updatedAt;
        }
    }
}