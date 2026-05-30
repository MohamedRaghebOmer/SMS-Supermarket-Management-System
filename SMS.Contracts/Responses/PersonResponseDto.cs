namespace SMS.Contracts.Responses
{
    public sealed record PersonResponseDto
    {
        public int PersonId { get; init; }
        public string NationalNo { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string SecondName { get; init; } = string.Empty;
        public string? ThirdName { get; init; }
        public string LastName { get; init; } = string.Empty;
        public DateTime DateOfBirth { get; init; }
        public SMS.Shared.Enums.Gender Gender { get; init; }
        public string Address { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public int NationalityCountryId { get; init; }
        public FileResponse? Image { get; init; }
    }
}
