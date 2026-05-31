using System.ComponentModel.DataAnnotations;

namespace SMS.Contracts.Requests.Customers
{
    public sealed record UpdateCustomerRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "PersonId must be greater than 0.")]
        public int PersonId { get; init; }

        [Range(1, 31, ErrorMessage = "PaymentDay must be between 1 and 31.")]
        public byte PaymentDay { get; init; }

        [StringLength(250)]
        public string? Notes { get; init; }
    }
}
