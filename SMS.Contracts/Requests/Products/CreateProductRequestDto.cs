using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Products
{
    public sealed record CreateProductRequestDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; init; }

        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string ProductName { get; init; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string SKU { get; init; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UnitId { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SellPrice { get; init; }

        [Required]
        [Range(0, 100)]
        public decimal DiscountPercent { get; init; }
    }
}
