using SMS.Application.Interfaces.Helpers;
using SMS.Shared.Common;
using SMS.Shared.Guards;
using System.ComponentModel.DataAnnotations;

namespace SMS.Application.Helpers
{
    public class ValidationHelper : IValidationHelper
    {
        public void ValidatePagination(PaginationRequest paginationRequest)
        {
            ArgumentNullException.ThrowIfNull(paginationRequest);
            NumericGuard.AgainstNonPositiveNumber(paginationRequest.Page,
                nameof(paginationRequest.Page));
            NumericGuard.AgainstNonPositiveNumber(paginationRequest.PageSize,
                nameof(paginationRequest.PageSize));
        }

        public void ValidateEmail(string? email, string parameterName, bool isRequired = true,
            int minLength = 5, int maxLength = 50)
        {
            if (!isRequired && string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            StringGuard.AgainstNullOrWhiteSpace(email ?? string.Empty, parameterName);
            StringGuard.EnsureLengthInRange(email!, minLength, maxLength, parameterName);

            if (!new EmailAddressAttribute().IsValid(email))
            {
                throw new ArgumentException("Invalid email format.", parameterName);
            }
        }
    }
}
