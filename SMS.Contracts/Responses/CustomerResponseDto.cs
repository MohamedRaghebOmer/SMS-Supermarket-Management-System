namespace SMS.Contracts.Responses
{
    public sealed record CustomerResponseDto
    {
        public int CustomerId { get; init; }
        public int PersonId { get; init; }
        public DateTime JoinDate { get; init; }
        public bool IsActive { get; init; }
        public byte PaymentDay { get; init; }
        public decimal CurrentBalance { get; init; }
        public DateTime? LastPaymentDate { get; init; }
        public DateTime? NextDueDate { get; init; }
        public string? Notes { get; init; }
    }
}
