namespace Jagabata.Resources;

public class SchedulePreview(string[] local, string[] utc)
{
    public const string PATH = "/api/v2/schedules/preview/";
    public string[] Local { get; } = local;
    public string[] Utc { get; } = utc;
}
