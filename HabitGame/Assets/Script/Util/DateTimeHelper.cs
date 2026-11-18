using System;
using System.Globalization;

public static class DateTimeHelper
{
    public static int ToYearMonth(DateTime date)
    {
        return date.Year * 100 + date.Month;
    }

    public static string ToDateKey(DateTime date)
    {
        return date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
    }

    public static int ToDateNumber(DateTime date)
    {
        return ToYearMonth(date) * 100 + date.Day;
    }

    public static DateTime ConvertToDateTime(long unixTimeMilliseconds)
    {
        if (unixTimeMilliseconds <= 0L)
        {
            return DateTime.MinValue;
        }

        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).LocalDateTime.Date;
    }
}
