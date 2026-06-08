namespace SMS.Contracts.Responses
{
    public sealed record ReturnItemResponseDto
    {
        public int ReturnItemId { get; init; }
        public int ReturnId { get; init; }
        public int SaleItemId { get; init; }
        public int ProductId { get; init; }
        public decimal Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal LineTotal { get; init; }
    }
}
