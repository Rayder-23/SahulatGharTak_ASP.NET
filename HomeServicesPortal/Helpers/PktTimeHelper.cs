namespace HomeServicesPortal.Helpers;

public static class PktTimeHelper
{
    private static readonly TimeSpan PktOffset = TimeSpan.FromHours(5); // PKT = UTC+5, no DST

    /// <summary>Converts a UTC DateTime to Pakistan Standard Time. Assumes input is UTC regardless of Kind.</summary>
    public static DateTime ToPkt(this DateTime utc) => DateTime.SpecifyKind(utc, DateTimeKind.Utc).Add(PktOffset);

    public static DateTime? ToPkt(this DateTime? utc) => utc.HasValue ? utc.Value.ToPkt() : null;

    /// <summary>Formats a UTC DateTime as PKT using the given .NET format string (e.g. "dd MMM yyyy HH:mm").</summary>
    public static string ToPktString(this DateTime utc, string format) => utc.ToPkt().ToString(format);

    public static string ToPktString(this DateTime? utc, string format, string fallback = "-") =>
        utc.HasValue ? utc.Value.ToPktString(format) : fallback;
}
