using System;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// Date time offset extensions
/// </summary>
internal static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Set the day to the last day at the end of the month
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>The date at the end of the month</returns>
    public static DateTimeOffset AtTheEndOfTheMonth(this DateTimeOffset date)
    {
        var day = DateTime.DaysInMonth(date.Year, date.Month);
        return new DateTimeOffset(date.Year, date.Month, day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Offset);
    }

    /// <summary>
    /// Set the day to midnight
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>The date at midnight</returns>
    public static DateTimeOffset AtMidnight(this DateTimeOffset date)
    {
        return new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Offset);
    }
}
