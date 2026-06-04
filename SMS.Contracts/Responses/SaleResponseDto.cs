using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public sealed record SaleResponseDto
    {
        public int SaleId { get; init; }
        public int? CustomerId { get; init; }
        public int CashierId { get; init; }
        public PaymentMethod? PaymentMethod { get; init; }
        public decimal SubTotal { get; init; }
        public decimal DiscountAmount { get; init; }
        public decimal NetTotal { get; init; }
        public decimal PaidAmount { get; init; }
        public decimal ChangeAmount { get; init; }
        public bool IsCredit { get; init; }
        public string? Notes { get; init; }
        public DateTime SaleDate { get; init; }
    }
}