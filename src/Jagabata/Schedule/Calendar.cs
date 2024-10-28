using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Jagabata.Schedule;

/// <summary>
/// For AWX/AnsibleTower Schedule
/// </summary>
public class Calendar : IParsable<Calendar>, ISpanParsable<Calendar>
{
    private Calendar()
    { }
    public Calendar(DateTime start)
    {
        TimeZoneInfo tz = TimeZoneInfo.Utc;
        if (start.Kind != DateTimeKind.Utc)
        {
            tz = TimeZoneInfo.Local;
        }
        TimeZone = tz;
        DTStart = new DateTimeOffset(start, tz.BaseUtcOffset);
    }
    public Calendar(DateTime start, TimeZoneInfo tz)
    {
        TimeZone = tz;
        DTStart = new DateTimeOffset(start, tz.BaseUtcOffset);
    }
    public Calendar(ReadOnlySpan<char> cal)
    {
        cal = cal.Trim();
        Range[] ranges = new Range[16];
        // split by Horizontal Tab(0x09) or Space(0x20)
        var len = cal.SplitAny(ranges, "\x09\x20", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (var i = 0; i < len; i++)
        {
            var val = cal[ranges[i]];
            if (val.StartsWith("DTSTART"))
            {
                ParseDTSTART(val[7..], this);
            }
            else if (val.StartsWith("RRULE:"))
            {
                RRules.Add(new RRule(val[6..]));
            }
            else if (val.StartsWith("EXRULE:"))
            {
                ExRRules.Add(new RRule(val[7..]));
            }
        }
    }
    private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;

    public DateTimeOffset DTStart { get; set; }
    public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;
    public List<RRule> RRules { get; } = [];
    public List<RRule> ExRRules { get; } = [];

    /// <summary>
    /// Parse <c>DTSTART***</c> and set <see cref="DTStart" /> and <see cref="TimeZone" />
    /// </summary>
    /// <param name="dtstart">string starts with <c>"DTSTART"</c></param>
    /// <param name="result">the object which is stored parsed result</param>
    /// <remarks>
    /// <c>DTSTART:yyyyMMddTHHmmssZ</c> Utc
    /// <c>DTSTART:yyyyMMddTHHmmss</c> Local
    /// <c>DTSTART;TZID=****:yyyyMMddTHHmmss</c> Local
    /// </remarks>
    private static void ParseDTSTART(ReadOnlySpan<char> dtstart, Calendar result)
    {
        DateTime datetime;
        TimeZoneInfo tzInfo = TimeZoneInfo.Utc;
        if (dtstart[0] == ':')
        {
            datetime = DateTime.ParseExact(dtstart[1..], "yyyyMMddTHHmmssK", invariantCulture, DateTimeStyles.AdjustToUniversal);
            tzInfo = datetime.Kind == DateTimeKind.Utc ? TimeZoneInfo.Utc : TimeZoneInfo.Local;
        }
        else if (dtstart[0] == ';')
        {
            var pos = dtstart.IndexOf(':');
            if (pos < 0)
            {
                throw new InvalidDataException($"Invald data: {dtstart}");
            }

            datetime = DateTime.ParseExact(dtstart[(pos + 1)..], "yyyyMMddTHHmmssK", invariantCulture, DateTimeStyles.AdjustToUniversal);
            if (datetime.Kind != DateTimeKind.Utc
                && dtstart.StartsWith(";TZID="))
            {
                tzInfo = TimeZoneInfo.FindSystemTimeZoneById(dtstart[6..pos].ToString());
            }

        }
        else
        {
            throw new InvalidDataException($"Invald data: {dtstart}");
        }

        result.DTStart = new DateTimeOffset(datetime, tzInfo.BaseUtcOffset);
        result.TimeZone = tzInfo;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        // DTSTART
        _ = sb.Append("DTSTART");
        _ = TimeZone.Equals(TimeZoneInfo.Utc)
            ? sb.Append(invariantCulture, $":{DTStart.ToString("yyyyMMddTHHmmssZ", invariantCulture)}")
            : sb.Append(invariantCulture, $";TZID={TimeZone.Id}:{DTStart.ToString("yyyyMMddTHHmmss", invariantCulture)}");

        foreach (RRule rule in RRules)
        {
            _ = sb.Append(" RRULE:")
                  .Append(rule.ToString());
        }
        foreach (RRule rule in ExRRules)
        {
            _ = sb.Append(" EXRULE:")
                  .Append(rule.ToString());
        }

        return sb.ToString();
    }

    public static Calendar Parse(string s, IFormatProvider? provider = null)
    {
        return new Calendar(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Calendar result)
    {
        try
        {
            result = new Calendar(s);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Calendar result)
    {
        return TryParse(s, out result);
    }

    public static Calendar Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return new Calendar(s);
    }

    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out Calendar result)
    {
        try
        {
            result = new Calendar(s);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Calendar result)
    {
        return TryParse(s, out result);
    }
}
