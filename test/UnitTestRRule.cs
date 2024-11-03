using Jagabata.Schedule;

namespace RRuleTest;

[TestClass]
public class DTStartTest
{
    [TestMethod]
    public void Test_1_Utc()
    {
        var cal = new Calendar("DTSTART:20241025T010203Z");

        var tz = TimeZoneInfo.Utc;
        Assert.AreEqual(tz, cal.TimeZone);
        Assert.AreEqual(tz.BaseUtcOffset, cal.DTStart.Offset);
        Assert.AreEqual(new DateTimeOffset(2024, 10, 25, 1, 2, 3, tz.BaseUtcOffset), cal.DTStart);
        Assert.AreEqual("DTSTART:20241025T010203Z", cal.ToString());
    }
    [TestMethod]
    public void Test_2_Local()
    {
        var cal = Calendar.Parse("DTSTART:20241025T010203");

        var tz = TimeZoneInfo.Local;
        Assert.AreEqual(TimeZoneInfo.Local, cal.TimeZone);
        Assert.AreEqual(TimeZoneInfo.Local.BaseUtcOffset, cal.DTStart.Offset);
        Assert.AreEqual(new DateTimeOffset(2024, 10, 25, 1, 2, 3, tz.BaseUtcOffset), cal.DTStart);
        Assert.AreEqual($"DTSTART;TZID={tz.Id}:20241025T010203", cal.ToString());
    }

    [TestMethod]
    public void Test_3_Local()
    {
        var cal = Calendar.Parse("DTSTART;TZID=America/New_York:20241025T010203");

        var tz = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        Assert.AreEqual(tz, cal.TimeZone);
        Assert.AreEqual(tz.BaseUtcOffset, cal.DTStart.Offset);
        Assert.AreEqual(new DateTimeOffset(2024, 10, 25, 1, 2, 3, tz.BaseUtcOffset), cal.DTStart);
        Assert.AreEqual($"DTSTART;TZID={tz.Id}:20241025T010203", cal.ToString());
    }

    [TestMethod]
    public void Test_4_RRule()
    {
        var cal = Calendar.Parse("DTSTART:20241025T010203Z RRULE:FREQ=MONTHLY;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-1");

        Assert.AreEqual(1, cal.RRules.Count);
        var rule = cal.RRules[0];
        Assert.AreEqual(RRuleFreq.MONTHLY, rule.Freq);
        Assert.AreEqual("MO,TU,WE,TH,FR", string.Join(',', rule.ByDay));
        Assert.AreEqual<short>(-1, rule.BySetPos[0]);
        Assert.AreEqual("FREQ=MONTHLY;INTERVAL=1;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-1", rule.ToString());

        Assert.AreEqual("DTSTART:20241025T010203Z RRULE:FREQ=MONTHLY;INTERVAL=1;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-1", cal.ToString());
    }

    [TestMethod]
    public void Test_5_RRule()
    {
        var cal = Calendar.Parse("RRULE:FREQ=YEARLY;BYDAY=-1MO RRULE:FREQ=MINUTELY;BYSECOND=20,30");

        Assert.AreEqual(2, cal.RRules.Count);

        var rule = cal.RRules[0];
        Assert.AreEqual(RRuleFreq.YEARLY, rule.Freq);
        Assert.AreEqual("FREQ=YEARLY;INTERVAL=1;BYDAY=-1MO", rule.ToString());

        rule = cal.RRules[1];
        Assert.AreEqual(RRuleFreq.MINUTELY, rule.Freq);
        Assert.AreEqual("FREQ=MINUTELY;INTERVAL=1;BYSECOND=20,30", rule.ToString());
    }

    [TestMethod]
    public void Test_6_Weekday()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeekDay.Parse("A"));
        Assert.IsFalse(WeekDay.TryParse("A", out var _));
        Assert.IsFalse(WeekDay.TryParse("1AA", out var _));
        Assert.IsTrue(WeekDay.TryParse("SU", out var _));
        Assert.IsTrue(WeekDay.TryParse("MO", out var _));
        Assert.IsTrue(WeekDay.TryParse("TU", out var _));
        Assert.IsTrue(WeekDay.TryParse("WE", out var _));
        Assert.IsTrue(WeekDay.TryParse("TH", out var _));
        Assert.IsTrue(WeekDay.TryParse("FR", out var _));
        Assert.IsTrue(WeekDay.TryParse("SA", out var _));

        Assert.IsTrue(WeekDay.TryParse("+1SU", out var _));
        Assert.IsTrue(WeekDay.TryParse("53SU", out var _));
        Assert.IsFalse(WeekDay.TryParse("54SU", out var _));
        Assert.IsTrue(WeekDay.TryParse("-1SU", out var _));
        Assert.IsTrue(WeekDay.TryParse("-53SU", out var _));
        Assert.IsFalse(WeekDay.TryParse("-54SU", out var _));
        Assert.IsFalse(WeekDay.TryParse("0SU", out var _));
    }
}
