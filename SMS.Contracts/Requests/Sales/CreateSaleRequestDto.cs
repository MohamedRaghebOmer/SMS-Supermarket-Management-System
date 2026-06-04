using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Sales
{
    public sealed record CreateSaleRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int? CustomerId { get; init; }

        public PaymentMethod? PaymentMethod { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "SubTotal cannot be negative.")]
        public decimal SubTotal { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "DiscountAmount cannot be negative.")]
        public decimal DiscountAmount { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "NetTotal cannot be negative.")]
        public decimal NetTotal { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "PaidAmount cannot be negative.")]
        public decimal PaidAmount { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "ChangeAmount cannot be negative.")]
        public decimal ChangeAmount { get; init; }

        public bool IsCredit { get; init; }

        [StringLength(250)]
        public string? Notes { get; init; }
    }
}