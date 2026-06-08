namespace SMS.Contracts.Responses
{
    public sealed record ReturnResponseDto
    {
        public int ReturnId { get; init; }
        public int SaleId { get; init; }
        public int? CustomerId { get; init; }
        public string? ReturnReason { get; init; }
        public decimal ReturnTotal { get; init; }
        public int CreatedBy { get; init; }
        public DateTime ReturnDate { get; init; }
    }
}
