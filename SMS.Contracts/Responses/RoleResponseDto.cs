namespace SMS.Contracts.Responses
{
    public sealed record RoleResponseDto
    {
        public int RoleId { get; init; }
        public string RoleName { get; init; } = string.Empty;
        public string? RoleDescription { get; init; }
        public bool IsActive { get; init; }
    }
}
