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
}
