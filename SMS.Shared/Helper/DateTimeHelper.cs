namespace SMS.Shared.Helper
{
    public static class DateTimeHelper
    {
        public static bool IsAdult(DateTime date)
        {
            var today = DateTime.Today;
            var age = today.Year - date.Year;

            if (date > today.AddYears(-age))
                age--;

            return age >= 18;
        }
    }
}
