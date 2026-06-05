namespace SMS.Contracts.Responses
{
    public sealed record ProductResponseDto
    {
        public int ProductId { get; init; }
        public int CategoryId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int UnitId { get; init; }
        public decimal CostPrice { get; init; }
        public decimal SellPrice { get; init; }
        public decimal DiscountPercent { get; init; }
        public Guid? ImageGuid { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
