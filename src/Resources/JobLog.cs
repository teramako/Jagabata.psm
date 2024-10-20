namespace Jagabata.Resources
{
    public class JobLog(JobLog.JobLogRange range, string content)
    {
        public JobLogRange Range { get; } = range;
        public string Content { get; } = content;
        public record JobLogRange(uint Start, uint End, uint AbsoluteEnd);
    }
}
