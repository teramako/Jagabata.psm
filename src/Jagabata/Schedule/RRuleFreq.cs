namespace Jagabata.Schedule;

/// <summary>
/// Type of recurrence rule.
/// </summary>
/// <seealso cref="RRule.Freq" />
public enum RRuleFreq
{
    /// <summary>
    /// Yearly: to specify repeating events based on an interval of a year or more
    /// </summary>
    YEARLY,
    /// <summary>
    /// Monthly: to specify repeating events based on an interval of a month or more
    /// </summary>
    MONTHLY,
    /// <summary>
    /// Weekly: to specify repeating events based on an interval of a week or more
    /// </summary>
    WEEKLY,
    /// <summary>
    /// Daily: to specify repeating events based on an interval of a day or more
    /// </summary>
    DAILY,
    /// <summary>
    /// Hourly: to specify repeating events based on an interval of an hour or more
    /// </summary>
    HOURLY,
    /// <summary>
    /// Minutely: to specify repeating events based on an interval of a minute or more
    /// </summary>
    MINUTELY,
}

