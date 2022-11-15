using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs;

/// <summary>
/// A time for tracking a year with a month only
/// </summary>
/// <remarks>
/// A 2 digit year is in the 1900's if it is between 60 - 99, thus 61 = 1961.
/// If it is between 1 - 59 it is in the 2000's, thus 25 = 2025
/// </remarks>
[JsonConverter(typeof(MonthOnlyConverter))]
public record struct MonthOnly
{
    /// <summary>
    /// Split between 2000's and 1900's, e.g. lower than this is the 2000's and higher (up to 100) is the 1900's
    /// </summary>
    private const int split = 60;

    /// <summary>
    /// Instantiate a new month only struct
    /// </summary>
    /// <param name="year">The year</param>
    /// <param name="month">The month</param>
    /// <remarks>
    /// A 2 digit year is in the 1900's if it is between 60 - 99, thus 61 = 1961.
    /// If it is between 1 - 59 it is in the 2000's, thus 25 = 2025
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if year or month is out of range</exception>
    public MonthOnly(
        int year,
        int month
    )
    {
        if (year == 0) year = 2000;
        if (year < 1 || year > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1 through 9999");
        }

        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 through 12");
        }

        if (year < split)
        {
            year += 2000;
        }
        else if (year >= split && year <= 99)
        {
            year += 1900;
        }

        Year = year;
        Month = month;
    }

    /// <summary>
    /// The year from 1 through 9999
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// The month
    /// </summary>
    public int Month { get; }

    /// <summary>
    /// Convert this month only to a date time
    /// </summary>
    /// <param name="monthly">The month only time</param>
    /// <remarks>
    /// Returns the last day in the month of the given month
    /// </remarks>
    public static implicit operator DateOnly(MonthOnly monthly) => new(monthly.Year, monthly.Month, DateTime.DaysInMonth(monthly.Year, monthly.Month));

    /// <summary>
    /// Convert this date only to a month only time (loses the day precision)
    /// </summary>
    /// <param name="date">The day only time</param>
    public static implicit operator MonthOnly(DateOnly date) => new(date.Year, date.Month);

    /// <summary>
    /// The less than operator. Time is measured forward in time starting from Year 1, where a year closer to zero is smaller than a year further away from zero.
    /// </summary>
    /// <param name="left">The left argument</param>
    /// <param name="right">The right argument</param>
    /// <returns>True if left is less than the right</returns>
    public static bool operator <(MonthOnly left, MonthOnly right) => (left.Year < right.Year) || ((left.Year == right.Year) && (left.Month < right.Month));

    /// <summary>
    /// The greater than operator. Time is measured forward in time starting from Year 1, where a year closer to zero is smaller than a year further away from zero.
    /// </summary>
    /// <param name="left">The left argument</param>
    /// <param name="right">The right argument</param>
    /// <returns>True if left is greater than the right</returns>
    public static bool operator >(MonthOnly left, MonthOnly right) => (left.Year > right.Year) || ((left.Year == right.Year) && (left.Month > right.Month));

    /// <summary>
    /// The string representation of month only
    /// </summary>
    /// <returns>A string of the YYMM format</returns>
    public override string ToString()
    {
        var yearText = (Year % 100).ToString("D2");
        var monthText = Month.ToString("D2");
        return monthText + yearText;
    }
}
