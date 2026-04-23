using System;
using System.Net.Mail;

namespace SMS.Core.Helpers
{
    public static class ValidationHelper
    {
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

        public static bool IsAdult(DateTime dateOfBirth)
        {
            return dateOfBirth < DateTime.Now.AddYears(-18);
        }
    }
}
