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

    }
}
