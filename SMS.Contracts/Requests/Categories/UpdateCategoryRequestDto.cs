using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Categories
{
    public sealed record UpdateCategoryRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 100 characters.")]
        public string CategoryName { get; init; } = string.Empty;

        [StringLength(250, ErrorMessage = "Category description cannot exceed 250 characters.")]
        public string? CategoryDescription { get; init; }

        public bool IsActive { get; init; }
    }
}