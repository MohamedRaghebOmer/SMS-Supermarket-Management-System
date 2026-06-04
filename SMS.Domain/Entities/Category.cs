using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class Category
    {
        private int _categoryId;
        private string _categoryName = null!;
        private string? _categoryDescription;

        public int CategoryId
        {
            get => _categoryId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _categoryId = value;
            }
        }

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(CategoryName));
                StringGuard.AgainstExcessiveLength(value, 100, nameof(CategoryName));
                _categoryName = value.Trim();
            }
        }

        public string? CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                StringGuard.AgainstExcessiveLength(value, 250, nameof(CategoryDescription));
                _categoryDescription = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        public bool IsActive { get; set; }

        public Category()
        {
        }

        public Category(string categoryName, string? categoryDescription, bool isActive)
        {
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            IsActive = isActive;
        }

        public Category(int categoryId, string categoryName, string? categoryDescription, bool isActive)
            : this(categoryName, categoryDescription, isActive)
        {
            CategoryId = categoryId;
        }
    }
}