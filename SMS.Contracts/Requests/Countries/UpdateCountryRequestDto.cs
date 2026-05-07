using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Countries
{
    public class UpdateCountryRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CountryName { get; set; } = string.Empty;
    }
}
