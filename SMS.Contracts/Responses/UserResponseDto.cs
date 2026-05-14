namespace SMS.Contracts.Responses
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}