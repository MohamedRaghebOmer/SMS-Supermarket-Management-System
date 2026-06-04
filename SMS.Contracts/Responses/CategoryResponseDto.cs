namespace SMS.Contracts.Responses
{
    public sealed record CategoryResponseDto
    {
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string? CategoryDescription { get; init; }
        public bool IsActive { get; init; }
    }
}
