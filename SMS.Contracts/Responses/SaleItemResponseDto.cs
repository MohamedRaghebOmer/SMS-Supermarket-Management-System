namespace SMS.Contracts.Responses
{
    public sealed record SaleItemResponseDto
    {
        public int SaleItemId { get; init; }
        public int SaleId { get; init; }
        public int ProductId { get; init; }
        public decimal Quantity { get; init; }
        public decimal UnitCostPriceAtSale { get; init; }
        public decimal UnitSellPriceAtSale { get; init; }
        public decimal DiscountAmount { get; init; }
        public decimal LineTotal { get; init; }
    }
}
