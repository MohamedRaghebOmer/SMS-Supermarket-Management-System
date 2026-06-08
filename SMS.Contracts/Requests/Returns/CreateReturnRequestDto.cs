using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Returns
{
    public sealed record CreateReturnRequestDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SaleId { get; init; }

        [Range(1, int.MaxValue)]
        public int? CustomerId { get; init; }

        [StringLength(250)]
        public string? ReturnReason { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ReturnTotal { get; init; }
    }
}
