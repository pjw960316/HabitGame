using System;

public static class DateTimeHelper
{
    public static int ToYearMonth(DateTime date)
    {
        return date.Year * 100 + date.Month;
    }
}
