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
            NumericGuard.AgainstInvalidId(paginationRequest.Page);
            NumericGuard.AgainstInvalidId(paginationRequest.PageSize);
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
