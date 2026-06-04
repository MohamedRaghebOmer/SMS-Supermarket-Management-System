using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Units
{
    public sealed record CreateUnitRequestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Unit name must be between 1 and 20 characters.")]
        [Description("The name of the unit to be created.")]
        public string UnitName { get; init; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Symbol must be between 1 and 10 characters.")]
        public string Symbol { get; init; } = string.Empty;

        public bool IsDecimal { get; init; }
    }
}