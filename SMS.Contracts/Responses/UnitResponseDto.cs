namespace SMS.Contracts.Responses
{
    public sealed record UnitResponseDto
    {
        public int UnitId { get; init; }
        public string UnitName { get; init; } = string.Empty;
        public string Symbol { get; init; } = string.Empty;
        public bool IsDecimal { get; init; }
    }
}
