namespace SMS.Application.Common.Guards
{
    internal class Guard
    {
        public static void AgainstInvalidId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
        }

        public static void AgainstNullOrEmptyString(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }
        }
    }
}
