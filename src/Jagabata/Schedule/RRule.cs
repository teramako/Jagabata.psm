using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Jagabata.Schedule;

/// <summary>
/// Recurrence Rule
/// </summary>
/// <seealso href="https://datatracker.ietf.org/doc/html/rfc5545#section-3.3.10">RFC 5545 - 3.3.10</seealso>
public class RRule : IParsable<RRule>, ISpanParsable<RRule>
{
    public RRule() : this(RRuleFreq.YEARLY)
    { }

    public RRule(RRuleFreq freq)
    {
        Freq = freq;
    }

    /// <summary>
    /// parse string
    /// <code>
    /// RRULE:key1=val1;key2=val2;...
    ///       ^^^^^^^^^^^^^^^^^^^^^^^
    /// </code>
    /// </summary>
    public RRule(ReadOnlySpan<char> rule)
    {
        Span<Range> partRanges = new Range[13];
        Span<Range> kvRange = new Range[2];
        Span<Range> valRange = new Range[128];
        /* int valLen; */
        int len = rule.Split(partRanges, ';');
        for (int i = 0; i < len; i++)
        {
            var recur = rule[partRanges[i]];
            if (recur.Split(kvRange, '=') != 2)
            {
                throw new InvalidDataException($"Invalid: {recur}");
            }
            var key = recur[kvRange[0]];
            var val = recur[kvRange[1]];
            switch (key)
            {
                case "FREQ":
                    Freq = Enum.Parse<RRuleFreq>(val, false);
                    continue;
                case "INTERVAL":
                    Interval = uint.Parse(val, invariantCulture);
                    continue;
                case "UNTIL":
                    Until = DateTime.ParseExact(val, "yyyyMMddTHHmmssK", invariantCulture, DateTimeStyles.AssumeUniversal);
                    continue;
                case "COUNT":
                    Count = uint.Parse(val, invariantCulture);
                    continue;
                case "BYSECOND":
                    _ = SetSecond(SplitComma<byte>(val, valRange));
                    continue;
                case "BYMINUTE":
                    _ = SetMinute(SplitComma<byte>(val, valRange));
                    continue;
                case "BYHOUR":
                    _ = SetHour(SplitComma<byte>(val, valRange));
                    continue;
                case "BYDAY":
                    _ = SetWeekDay(SplitComma<WeekDay>(val, valRange));
                    continue;
                case "BYMONTHDAY":
                    _ = SetMonthDay(SplitComma<sbyte>(val, valRange));
                    continue;
                case "BYYEARDAY":
                    _ = SetYearDay(SplitComma<short>(val, valRange));
                    continue;
                case "BYWEEKNO":
                    _ = SetWeekNo(SplitComma<sbyte>(val, valRange));
                    continue;
                case "BYMONTH":
                    _ = SetMonth(SplitComma<sbyte>(val, valRange));
                    continue;
                case "BYSETPOS":
                    _ = SetPos(SplitComma<short>(val, valRange));
                    continue;
                default:
                    throw new InvalidDataException($"Invalid Recurrence Key: {key}");
            }
        }
    }
    private static T[] SplitComma<T>(ReadOnlySpan<char> val, Span<Range> bufRange) where T : struct, ISpanParsable<T>
    {
        int len = val.Split(bufRange, ',');
        T[] result = new T[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = T.Parse(val[bufRange[i]], invariantCulture);
        }
        return result;
    }
    private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// The FREQ rule part identifies the type of recurrence rule.
    /// This rule part MUST be specified in the recurrence rule.
    /// </summary>
    public RRuleFreq Freq { get; set; }
    /// <summary>
    /// The INTERVAL rule part contains a positive integer representing at
    /// which intervals the recurrence rule repeats.  The default value is
    /// "1", meaning every second for a SECONDLY rule, every minute for a
    /// MINUTELY rule, every hour for an HOURLY rule, every day for a
    /// DAILY rule, every week for a WEEKLY rule, every month for a
    /// MONTHLY rule, and every year for a YEARLY rule.  For example,
    /// within a DAILY rule, a value of "8" means every eight days.
    /// </summary>
    public uint Interval { get; set; } = 1;
    /// <summary>
    /// The COUNT rule part defines the number of occurrences at which to
    /// range-bound the recurrence.  The "DTSTART" property value always
    /// counts as the first occurrence.
    /// </summary>
    public uint? Count { get; set; }
    /// <summary>
    /// The UNTIL rule part defines a DATE or DATE-TIME value that bounds
    /// the recurrence rule in an inclusive manner.
    /// If the value specified by UNTIL is synchronized with the specified recurrence,
    /// this DATE or DATE-TIME becomes the last instance of the recurrence.
    /// </summary>
    /// <remarks>
    /// Implemented stored as UTC datetime always.
    /// </remarks>
    public DateTime? Until
    {
        get => _until;
        set
        {
            if (value is null)
            {
                _until = null;
                return;
            }
            if (value is DateTime dt)
            {
                _until = dt.Kind != DateTimeKind.Utc ? dt.ToUniversalTime() : dt;
                return;
            }
            throw new InvalidDataException($"value must be null or {nameof(DateTime)}: {value}");
        }
    }
    private DateTime? _until;

    /// <summary>
    /// The BYSECOND rule part specifies a COMMA-separated list of seconds within a minute.
    /// Valid values are 0 to 60.
    /// </summary>
    public IReadOnlyList<byte> BySecond => _seconds;
    private byte[] _seconds = [];
    /// <summary>
    /// The BYMINUTE rule part specifies a COMMA-separated list of minutes within an hour.
    /// Valid values are 0 to 59.
    /// </summary>
    public IReadOnlyList<byte> ByMinute => _minutes;
    private byte[] _minutes = [];
    /// <summary>
    /// The BYHOUR rule part specifies a COMMA-separated list of hours of the day.
    /// Valid values are 0 to 23.
    /// </summary>
    public IReadOnlyList<byte> ByHour => _hours;
    private byte[] _hours = [];
    /// <summary>
    /// The BYDAY rule part specifies a COMMA-separated list of days of
    /// the week; SU indicates Sunday; MO indicates Monday; TU indicates
    /// Tuesday; WE indicates Wednesday; TH indicates Thursday; FR
    /// indicates Friday; and SA indicates Saturday.
    /// </summary>
    /// <remarks>
    /// Each BYDAY value can also be preceded by a positive (+n) or
    /// negative (-n) integer.  If present, this indicates the nth
    /// occurrence of a specific day within the MONTHLY or YEARLY "RRULE".
    ///
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
    /// </remarks>
    public IReadOnlyList<WeekDay> ByDay => _weekDays;
    private WeekDay[] _weekDays = [];
    /// <summary>
    /// The BYMONTHDAY rule part specifies a COMMA-separated list of days of the month.
    /// Valid values are 1 to 31 or -31 to -1.
    /// </summary>
    /// <remarks>
    /// For example, -10 represents the tenth to the last day of the month.
    /// The BYMONTHDAY rule part MUST NOT be specified when the FREQ rule part is set to WEEKLY.
    /// </remarks>
    public IReadOnlyList<sbyte> ByMonthDay => _monthDays;
    private sbyte[] _monthDays = [];
    /// <summary>
    /// The BYYEARDAY rule part specifies a COMMA-separated list of days of the year.
    /// Valid values are 1 to 366 or -366 to -1.
    /// </summary>
    /// <remarks>
    /// For example, -1 represents the last day of the year (December 31st)
    /// and -306 represents the 306th to the last day of the year (March 1st).
    /// The BYYEARDAY rule part MUST NOT be specified when the FREQ
    /// rule part is set to DAILY, WEEKLY, or MONTHLY.
    /// </remarks>
    public IReadOnlyList<short> ByYearDay => _yearDays;
    private short[] _yearDays = [];
    /// <summary>
    /// The BYWEEKNO rule part specifies a COMMA-separated list of ordinals specifying weeks of the year.
    /// Valid values are 1 to 53 or -53 to -1.
    /// </summary>
    /// <remarks>
    /// This corresponds to weeks according to week numbering as defined in [ISO.8601.2004].
    /// A week is defined as a seven day period, starting on the day of the week defined to be
    /// the week start (see WKST).  Week number one of the calendar year is the first week that
    /// contains at least four (4) days in that calendar year.
    /// This rule part MUST NOT be used when the FREQ rule part is set to anything other than YEARLY.
    /// For example, 3 represents the third week of the year.
    ///
    /// <code>
    /// Note: Assuming a Monday week start, week 53 can only occur when
    ///       Thursday is January 1 or if it is a leap year and Wednesday is January 1.
    ///       BYWEEKNO: -53- -1, 1 - 53
    /// </code>
    /// </remarks>
    public IReadOnlyList<sbyte> ByWeekNo => _weekNumbers;
    private sbyte[] _weekNumbers = [];
    /// <summary>
    /// The BYMONTH rule part specifies a COMMA-separated list of months of the year.
    /// Valid values are 1 to 12.
    /// </summary>
    public IReadOnlyList<sbyte> ByMonth => _months;
    private sbyte[] _months = [];
    /// <summary>
    /// The BYSETPOS rule part specifies a COMMA-separated list of values
    /// that corresponds to the nth occurrence within the set of
    /// recurrence instances specified by the rule.
    /// BYSETPOS operates on a set of recurrence instances in one interval of the recurrence rule.
    /// For example, in a WEEKLY rule, the interval would be one week A set of recurrence
    /// instances starts at the beginning of the interval defined by the FREQ rule part.
    /// Valid values are 1 to 366 or -366 to -1.
    /// It MUST only be used in conjunction with another BYxxx rule part.
    /// For example "the last work day of the month" could be represented as:
    /// <code>
    ///   FREQ=MONTHLY;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-1
    /// </code>
    /// Each BYSETPOS value can include a positive (+n) or negative (-n) integer.
    /// If present, this indicates the nth occurrence of the specific occurrence
    /// within the set of occurrences specified by the rule.
    /// </summary>
    public IReadOnlyList<short> BySetPos => _setPostions;
    private short[] _setPostions = [];

    /// <summary>
    /// Reset all data
    /// </summary>
    /// <returns>this object</returns>
    public RRule Reset()
    {
        Freq = RRuleFreq.YEARLY;
        Interval = 1;
        Count = null;
        Until = null;
        _seconds = [];
        _minutes = [];
        _hours = [];
        _weekDays = [];
        _monthDays = [];
        _yearDays = [];
        _weekNumbers = [];
        _months = [];
        _setPostions = [];
        return this;
    }

    /// <summary>
    /// Add to <c>BySecond</c>
    /// </summary>
    /// <param name="byseclist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between 0 and 60</exception>
    public RRule SetSecond(params byte[] byseclist)
    {
        if (byseclist.Any(static sec => sec > 60))
        {
            throw new InvalidDataException("Second must be between 0 and 60");
        }
        _seconds = byseclist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByMinute</c>
    /// </summary>
    /// <param name="byminlist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between 0 and 59</exception>
    public RRule SetMinute(params byte[] byminlist)
    {
        if (byminlist.Any(static min => min > 59))
        {
            throw new InvalidDataException("Minute must be between 0 and 59");
        }
        _minutes = byminlist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByHour</c>
    /// </summary>
    /// <param name="byhrlist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between 0 and 59</exception>
    public RRule SetHour(params byte[] byhrlist)
    {
        if (byhrlist.Any(static hr => hr > 23))
        {
            throw new InvalidDataException("Hour must be between 0 and 23");
        }
        _hours = byhrlist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByMonthDay</c>
    /// </summary>
    /// <param name="bymodaylist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between -31 and -1 or between 1 and 31</exception>
    public RRule SetMonthDay(params sbyte[] bymodaylist)
    {
        if (bymodaylist.Any(static moday => moday is > 31 or 0 or < (-31)))
        {
            throw new InvalidDataException("Month Day must be between -31 and -1 or between 1 and 31");
        }
        _monthDays = bymodaylist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByYearDay</c>
    /// </summary>
    /// <param name="byyrdaylist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between -31 and -1 or between 1 and 31</exception>
    public RRule SetYearDay(params short[] byyrdaylist)
    {
        if (byyrdaylist.Any(static yrday => yrday is > 366 or 0 or < (-366)))
        {
            throw new InvalidDataException("Year Day must be between -366 and -1 or between 1 and 366");
        }
        _yearDays = byyrdaylist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByWeekNo</c>
    /// </summary>
    /// <param name="bywknolist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between -31 and -1 or between 1 and 31</exception>
    public RRule SetWeekNo(params sbyte[] bywknolist)
    {
        if (bywknolist.Any(static wkno => wkno is > 53 or 0 or < (-53)))
        {
            throw new InvalidDataException($"Week No must be between -366 and -1 or between 1 and 366");
        }
        _weekNumbers = bywknolist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByMonth</c>
    /// </summary>
    /// <param name="bymolist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between -31 and -1 or between 1 and 31</exception>
    public RRule SetMonth(params sbyte[] bymolist)
    {
        if (bymolist.Any(static mo => mo is > 12 or 0 or < (-12)))
        {
            throw new InvalidDataException($"Month must be between -12 and -1 or between 1 and 12");
        }
        _months = bymolist;
        return this;
    }

    /// <summary>
    /// Add to <c>BySetPos</c>
    /// </summary>
    /// <param name="bysplist"></param>
    /// <returns>this object</returns>
    /// <exception cref="InvalidDataException">thrown when any arguments with not between -31 and -1 or between 1 and 31</exception>
    public RRule SetPos(params short[] bysplist)
    {
        if (bysplist.Any(static sp => sp is > 366 or 0 or < (-366)))
        {
            throw new InvalidDataException($"SetPos must be between -366 and -1 or between 1 and 366");
        }
        _setPostions = bysplist;
        return this;
    }

    /// <summary>
    /// Add to <c>ByDay</c>
    /// </summary>
    /// <param name="bywkdaylist">weekday</param>
    /// <returns>this object</returns>
    public RRule SetWeekDay(params WeekDay[] bywkdaylist)
    {
        _weekDays = bywkdaylist;
        return this;
    }
    /// <summary>
    /// Add to <c>ByDay</c>
    /// </summary>
    /// <param name="byweeklist">week</param>
    /// <returns>this object</returns>
    public RRule SetWeekDay(params RRuleWeek[] byweeklist)
    {
        _weekDays = byweeklist.Select(static week => new WeekDay(week)).ToArray();
        return this;
    }
    /// <summary>
    /// Add to <c>ByDay</c>
    /// </summary>
    /// <param name="bywkdaylist">week</param>
    /// <returns>this object</returns>
    public RRule SetWeekDay(params string[] bywkdaylist)
    {
        _weekDays = bywkdaylist.Select(static wkday => new WeekDay(wkday)).ToArray();
        return this;
    }

    /// <remarks>
    /// <code>
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |          |SECONDLY|MINUTELY|HOURLY |DAILY  |WEEKLY|MONTHLY|YEARLY|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYMONTH   |Limit   |Limit   |Limit  |Limit  |Limit |Limit  |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYWEEKNO  |N/A     |N/A     |N/A    |N/A    |N/A   |N/A    |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYYEARDAY |Limit   |Limit   |Limit  |N/A    |N/A   |N/A    |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYMONTHDAY|Limit   |Limit   |Limit  |Limit  |N/A   |Expand |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYDAY     |Limit   |Limit   |Limit  |Limit  |Expand|Note 1 |Note 2|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYHOUR    |Limit   |Limit   |Limit  |Expand |Expand|Expand |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYMINUTE  |Limit   |Limit   |Expand |Expand |Expand|Expand |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYSECOND  |Limit   |Expand  |Expand |Expand |Expand|Expand |Expand|
    /// +----------+--------+--------+-------+-------+------+-------+------+
    /// |BYSETPOS  |Limit   |Limit   |Limit  |Limit  |Limit |Limit  |Limit |
    /// +----------+--------+--------+-------+-------+------+-------+------+
    ///
    /// Note 1:  Limit if BYMONTHDAY is present; otherwise, special expand
    ///          for MONTHLY.
    ///
    /// Note 2:  Limit if BYYEARDAY or BYMONTHDAY is present; otherwise,
    ///          special expand for WEEKLY if BYWEEKNO present; otherwise,
    ///          special expand for MONTHLY if BYMONTH present; otherwise,
    ///          special expand for YEARLY.
    /// </code>
    /// </remarks>
    public override string ToString()
    {
        StringBuilder sb = new();
        _ = sb.Append(invariantCulture, $"FREQ={Freq.ToString().ToUpperInvariant()}");
        _ = sb.Append(invariantCulture, $";INTERVAL={Interval}");
        switch (Freq)
        {
            case RRuleFreq.YEARLY:
                ToStringYearly(sb);
                break;
            case RRuleFreq.MONTHLY:
                ToStringMonthly(sb);
                break;
            case RRuleFreq.WEEKLY:
                ToStringWeekly(sb);
                break;
            case RRuleFreq.DAILY:
                ToStringWeekly(sb);
                break;
            case RRuleFreq.HOURLY:
                ToStringHourly(sb);
                break;
            case RRuleFreq.MINUTELY:
                ToStringMinutely(sb);
                break;
            default:
                break;
        }
        if (_setPostions.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYSETPOS={string.Join(',', _setPostions)}");
        }
        if (Count > 0)
        {
            _ = sb.Append(invariantCulture, $";COUNT={Count}");
        }
        if (Until is not null)
        {
            var datetime = (DateTime)Until;
            if (datetime.Kind != DateTimeKind.Utc)
            {
                datetime = datetime.ToUniversalTime();
            }
            _ = sb.Append(invariantCulture, $";UNTIL={datetime.ToString("yyyyMMddTHHmmssZ", invariantCulture)}");
        }
        return sb.ToString();
    }
    private void ToStringMinutely(StringBuilder sb)
    {
        if (_seconds.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYSECOND={string.Join(',', _seconds)}");
        }
    }
    private void ToStringHourly(StringBuilder sb)
    {
        ToStringMinutely(sb);
        if (_minutes.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYMINUTE={string.Join(',', _minutes)}");
        }
    }
    private void ToStringDaily(StringBuilder sb)
    {
        ToStringHourly(sb);
        if (_hours.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYHOUR={string.Join(',', _hours)}");
        }
    }
    private void ToStringWeekly(StringBuilder sb)
    {
        ToStringDaily(sb);
        if (_weekDays.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYDAY={string.Join(',', _weekDays)}");
        }

    }
    /// <remarks>
    /// Note 1:  Limit if BYMONTHDAY is present; otherwise, special expand
    ///          for MONTHLY.
    /// </remarks>
    private void ToStringMonthly(StringBuilder sb)
    {
        if (_monthDays.Length > 0)
        {
            ToStringHourly(sb);
            _ = sb.Append(invariantCulture, $";BYMONTHDAY={string.Join(',', _monthDays)}");
        }
        else
        {
            // Special expand ByDay for Monthly
            ToStringWeekly(sb);
        }
    }
    ///
    /// <remarks>
    /// Note 2:  Limit if BYYEARDAY or BYMONTHDAY is present; otherwise,
    ///          special expand for WEEKLY if BYWEEKNO present; otherwise,
    ///          special expand for MONTHLY if BYMONTH present; otherwise,
    ///          special expand for YEARLY.
    /// </remaks>
    private void ToStringYearly(StringBuilder sb)
    {
        if (_yearDays.Length > 0 || _monthDays.Length > 0)
        {
            // Limit ByDay
            ToStringDaily(sb);
        }
        else
        {
            ToStringWeekly(sb);
        }

        if (_monthDays.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYMONTHDAY={string.Join(',', _monthDays)}");
        }
        if (_yearDays.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYYEARDAY={string.Join(',', _yearDays)}");
        }
        if (_weekNumbers.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYWEEKNO={string.Join(',', _weekNumbers)}");
        }
        if (_months.Length > 0)
        {
            _ = sb.Append(invariantCulture, $";BYMONTH={string.Join(',', _months)}");
        }
    }

    public static RRule Parse(string s, IFormatProvider? provider = null)
    {
        return new RRule(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RRule result)
    {
        return TryParse(s, out result);
    }
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out RRule result)
    {
        try
        {
            result = new RRule(s);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static RRule Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return new RRule(s);
    }

    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out RRule result)
    {
        try
        {
            result = new RRule(s);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out RRule result)
    {
        return TryParse(s, out result);
    }
}
