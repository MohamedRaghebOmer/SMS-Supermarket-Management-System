namespace SMS.Shared.Guards
{
    public class DateGuard
    {
        public static void AgainstFutureDate(DateTime date, string parameterName)
        {
            if (date > DateTime.UtcNow)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be a future date.");
            }
        }


        public static void AgainstPastDate(DateTime date, string parameterName)
        {
            if (date < DateTime.UtcNow)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be a past date.");
            }
        }

        public static void AgainstInvalidDateRange(DateTime startDate, DateTime endDate, string startParameterName, string endParameterName)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException($"{startParameterName} cannot be later than {endParameterName}.");
            }
        }

        public static void EnsureIsAdult(DateTime dateOfBirth, string parameterName, int adultAge = 18)
        {
            var age = DateTime.UtcNow.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.UtcNow.AddYears(-age)) age--;
            if (age < adultAge)
            {
                throw new ArgumentException($"{parameterName} indicates an age of {age}, which is below the required adult age of {adultAge}.");
            }
        }
    }
}
