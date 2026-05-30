using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Countries
{
    public sealed record CreateCountryRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Description("The name of the country to be created.")]
        [Display(Name = "Country Name")]
        public string CountryName { get; init; } = string.Empty;
    }
}
