using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Countries
{
    public sealed record UpdateCountryRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CountryName { get; init; } = string.Empty;
    }
}
