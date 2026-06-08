using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.ReturnItems
{
    public sealed record CreateReturnItemRequestDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ReturnId { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SaleItemId { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LineTotal { get; init; }
    }
}
