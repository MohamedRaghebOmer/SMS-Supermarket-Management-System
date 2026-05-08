using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Shared.Guards
{
    public class StringGuard
    {
        public static void AgainstNullOrEmptyString(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }
        }

    }
}
