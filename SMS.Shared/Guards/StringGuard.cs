namespace SMS.Shared.Guards
{
    public class StringGuard
    {
        public static void AgainstNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }
        }


        public static void AgainstExcessiveLength(string value, int maxLength, string parameterName)
        {
            if (value != null && value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} cannot exceed {maxLength} characters.", parameterName);
            }
        }

        public static void AgainstShortLength(string value, int minLength, string parameterName)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} must be at least {minLength} characters long.", parameterName);
            }
        }

        public static void EnsureLengthInRange(string value, int minLength, int maxLength, string parameterName)
        {
            if (value != null)
            {
                if (value.Length < minLength || value.Length > maxLength)
                {
                    throw new ArgumentException($"{parameterName} must be between {minLength} and {maxLength} characters long.", parameterName);
                }
            }
        }
    }
}
