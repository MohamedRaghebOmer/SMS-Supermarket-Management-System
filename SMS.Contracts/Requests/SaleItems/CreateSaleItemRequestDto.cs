using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.SaleItems
{
    public sealed record CreateSaleItemRequestDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SaleId { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitSellPriceAtSale { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LineTotal { get; init; }
    }
}
