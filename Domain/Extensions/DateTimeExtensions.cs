namespace Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToStringDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
