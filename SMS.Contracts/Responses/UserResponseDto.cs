namespace SMS.Contracts.Responses
{
    public sealed record UserResponseDto
    {
        public int UserId { get; init; }
        public int PersonId { get; init; }
        public string UserName { get; init; } = null!;
        public int RoleId { get; init; }
        public bool IsActive { get; init; }
    }
}