namespace SMS.Contracts.Responses
{
    public sealed record CountryResponseDto
    {
        public int CountryId { get; init; }
        public string CountryName { get; init; } = string.Empty;
    }
}
