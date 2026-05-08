namespace SMS.Shared.Guards
{
    public class NumericGuard
    {
        public static void AgainstInvalidId(decimal id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
        }

        public static void AgainstInvalidId(decimal? id)
        {
            if (id.HasValue && id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
        }


        public static void AgainstNonPositiveNumber(decimal number, string parameterName)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be greater than zero.");
            }
        }

        public static void AgainstNegativeNumber(decimal number, string parameterName)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be negative.");
            }
        }
    }
}
