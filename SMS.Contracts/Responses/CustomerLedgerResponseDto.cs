using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public sealed record CustomerLedgerResponseDto
    {
        public int LedgerId { get; init; }
        public int CustomerId { get; init; }
        public DateTime EntryDate { get; init; }
        public CustomerLedgerEntryType EntryType { get; init; }
        public CustomerLedgerReferenceType ReferenceType { get; init; }
        public int? ReferenceId { get; init; }
        public decimal DebitAmount { get; init; }
        public decimal CreditAmount { get; init; }
        public decimal BalanceBefore { get; init; }
        public decimal BalanceAfter { get; init; }
        public int CreatedBy { get; init; }
        public string? Notes { get; init; }
    }
}
