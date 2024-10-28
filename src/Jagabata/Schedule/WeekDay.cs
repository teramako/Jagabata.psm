using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Jagabata.Schedule;

/// <summary>
/// <para>The BYDAY rule part specifies a COMMA-separated list of days of the week;</para>
/// <para>
///     SU indicates Sunday;
///     MO indicates Monday;
///     TU indicates Tuesday;
///     WE indicates Wednesday;
///     TH indicates Thursday;
///     FR indicates Friday;
///     and SA indicates Saturday.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// Each BYDAY value can also be preceded by a positive (+n) or
/// negative (-n) integer.  If present, this indicates the nth
/// occurrence of a specific day within the MONTHLY or YEARLY "RRULE".
/// </para>
/// <para>
/// For example, within a MONTHLY rule, +1MO (or simply 1MO)
/// represents the first Monday within the month, whereas -1MO
/// represents the last Monday of the month.  The numeric value in a
/// BYDAY rule part with the FREQ rule part set to YEARLY corresponds
/// to an offset within the month when the BYMONTH rule part is
/// present, and corresponds to an offset within the year when the
/// BYWEEKNO or BYMONTH rule parts are present.  If an integer
/// modifier is not present, it means all days of this type within the
/// specified frequency.  For example, within a MONTHLY rule, MO
/// represents all Mondays within the month.  The BYDAY rule part MUST
/// NOT be specified with a numeric value when the FREQ rule part is
/// not set to MONTHLY or YEARLY.  Furthermore, the BYDAY rule part
/// MUST NOT be specified with a numeric value with the FREQ rule part
/// set to YEARLY when the BYWEEKNO rule part is specified.
/// </para>
/// </remarks>
/// <seealso cref="RRule.ByDay" />
/// <seealso cref="RRuleWeek"/>
public struct WeekDay : IParsable<WeekDay>, ISpanParsable<WeekDay>
{
    public WeekDay(ReadOnlySpan<char> str)
    {
        int len = str.Length;
        if (len == 2)
        {
            Ord = 0;
            Week = Enum.Parse<RRuleWeek>(str, false);
        }
        else
        {
            Ord = sbyte.Parse(str[..(len - 2)], CultureInfo.InvariantCulture);
            if (Ord is > 53 or 0 or < (-53))
            {
                throw new ArgumentOutOfRangeException(nameof(str), $"Must be between -53 and -1 or between 1 and 53: {Ord}");
            }
            Week = Enum.Parse<RRuleWeek>(str[(len - 2)..], false);
        }
    }
    public WeekDay(RRuleWeek week, sbyte ord = 0)
    {
        if (ord is > 53 or < (-53))
        {
            throw new ArgumentOutOfRangeException(nameof(ord), $"Must be between -53 and -1 or between 1 and 53: {ord}");
        }
        Ord = ord;
        Week = week;
    }

    public sbyte Ord { get; set; }
    public RRuleWeek Week { get; set; }

    public static WeekDay Parse(string s, IFormatProvider? provider = null)
    {
        return new WeekDay(s);
    }

    public static WeekDay Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return new WeekDay(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out WeekDay result)
    {
        try
        {
            result = new WeekDay(s);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out WeekDay result)
    {
        return TryParse(s, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out WeekDay result)
    {
        try
        {
            result = new WeekDay(s);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out WeekDay result)
    {
        return TryParse(s, out result);
    }

    public override readonly string ToString()
    {
        return (Ord == 0) ? $"{Week}" : $"{Ord}{Week}";
    }
}
