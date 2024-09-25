namespace Timely.Domain.Extensions;

using Functional;

/// <summary>
/// Extensions to improve the composability of TimeSpan in this application.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Minimum finds the smaller of the two TimeSpans.
    /// </summary>
    /// <param name="t1">The original TimeSpan.</param>
    /// <param name="t2">The TimeSpan to compare.</param>
    /// <returns>The smaller of the two TimeSpans.</returns>
    public static TimeSpan Minimum(TimeSpan t1, TimeSpan t2) => t1 < t2 ? t1 : t2;

    /// <summary>
    /// Maximum finds the larger of the two TimeSpans.
    /// </summary>
    /// <param name="t1">The first TimeSpan.</param>
    /// <param name="t2">The TimeSpan to compare to.</param>
    /// <returns>The larger of the two TimeSpans.</returns>
    public static TimeSpan Maximum(TimeSpan t1, TimeSpan t2) => t1 > t2 ? t1 : t2;

    /// <summary>
    /// Convert a TimeSpan into a string representation.
    /// <example>
    /// <br/><br/>
    /// Example:
    /// <code>
    /// string displayTime = TimeSpan.FromSeconds(10).ToDisplay("HH:mm:ss");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToDisplay(this TimeSpan timeSpan, string format) =>
        new DateTime(timeSpan.Ticks).ToString(format);

    /// <summary>
    /// Convert a TimeSpan into a string representation.
    /// </summary>
    /// <param name="timeSpan">The time span to get the display value for.</param>
    /// <returns>A string representation of the time span.</returns>
    public static string ToDisplay(this TimeSpan timeSpan) =>
        (timeSpan as TimeSpan?)
            .DynamicTimeDisplayFormat()
            .Pipe(format => new DateTime(timeSpan.Ticks).ToString(format));

    /// <summary>
    /// Get the string display format for a TimeSpan based on how much time is present.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to get the display format for.</param>
    /// <returns>A format string that will display only the amount of time necessary.</returns>
    public static string DynamicTimeDisplayFormat(this TimeSpan? timeSpan)
    {
        var time = timeSpan ?? TimeSpan.Zero;

        var format = "";
        if (time.Hours > 0)
        {
            format += "HH'h '";
        }

        if (time.Hours > 0 || time.Minutes > 0)
        {
            format += "mm'm '";
        }

        format += "ss's'";

        return format;
    }
}
