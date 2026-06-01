using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.CustomerLedgers
{
    public sealed record CreateCustomerLedgerRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int CustomerId { get; init; }

        [Required]
        public CustomerLedgerEntryType EntryType { get; init; }

        [Required]
        public CustomerLedgerReferenceType ReferenceType { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "ReferenceId must be greater than 0.")]
        public int? ReferenceId { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "DebitAmount cannot be negative.")]
        public decimal DebitAmount { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "CreditAmount cannot be negative.")]
        public decimal CreditAmount { get; init; }

        [StringLength(250)]
        public string? Notes { get; init; }
    }
}
