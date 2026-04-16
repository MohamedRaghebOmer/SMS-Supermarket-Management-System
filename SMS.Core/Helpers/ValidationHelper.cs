using SMS.Core.DTOs;
using System;
using System.Net.Mail;

namespace SMS.Core.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(message, nameof(obj));
            }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void ValidateIntGreaterThanZero(int value, string message)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(message, nameof(value));
            }
        }

        public static void ValidateRequiredString(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, nameof(value));
            }
        }

        public static bool IsAdult(DateTime dateOfBirth)
        {
            return dateOfBirth < DateTime.Now.AddYears(-18);
        }
    }
}
