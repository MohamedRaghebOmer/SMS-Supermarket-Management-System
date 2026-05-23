namespace SMS.Contracts.Responses
{
    public class PersonResponseDto
    {
        public int PersonId { get; set; }
        public string NationalNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public SMS.Shared.Enums.Gender Gender { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int NationalityCountryId { get; set; }
        public Stream? Image { get; set; }
    }
}
