using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
