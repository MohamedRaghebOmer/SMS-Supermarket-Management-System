using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Helpers
{
    public interface IValidationHelper
    {
        void ValidatePagination(PaginationRequest paginationRequest);

        void ValidateEmail(string? email, string parameterName, bool isRequired = true,
            int minLength = 5, int maxLength = 50);

        void ValidateEnum<T>(T value, Type enumType) where T : struct, Enum;
    }
}