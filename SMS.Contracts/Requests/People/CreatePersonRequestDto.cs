using SMS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.People
{
    public class CreatePersonRequestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string NationalNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string SecondName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ThirdName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NationalityCountryId { get; set; }
    }
}
