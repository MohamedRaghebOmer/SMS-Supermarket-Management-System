using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.People
{
    public sealed record CreatePersonRequestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string NationalNo { get; init; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string SecondName { get; init; } = string.Empty;

        [StringLength(50)]
        public string? ThirdName { get; init; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; init; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; init; }

        [Required]
        public Gender Gender { get; init; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; init; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Phone { get; init; } = string.Empty;

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NationalityCountryId { get; init; }
    }
}
